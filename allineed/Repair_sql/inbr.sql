##DELIMITER |

drop procedure if exists `DetectInbreeding`|
CREATE PROCEDURE `DetectInbreeding`(rabGenoms1 TEXT,rabGenoms2 TEXT,OUT inbr BOOLEAN)
root:BEGIN
	DECLARE mGens,fGens TEXT DEFAULT '';
	SET max_sp_recursion_depth=50;
	
	IF LOCATE(rabGenoms1,rabGenoms2)>0 THEN 
		SET inbr=true;
	ELSE
		CALL ParceGenoms(rabGenoms1,mGens,fGens);
		IF mGens !='' THEN
			CALL DetectInbreeding(mGens,rabGenoms2,inbr);
			IF inbr THEN LEAVE root; END IF;
		END IF;
		IF fGens !='' THEN
			CALL DetectInbreeding(fGens,rabGenoms2,inbr);
			IF inbr THEN LEAVE root; END IF;
		END IF;
		SET inbr=false;
	END IF;
END |

drop procedure if exists `FindRabGenoms`|
CREATE PROCEDURE `FindRabGenoms`(IN rid INT UNSIGNED,INOUT depth SMALLINT UNSIGNED,OUT genoms TEXT)
    READS SQL DATA
root:BEGIN
	DECLARE cnt,mId,fId,mDpt,fDpt INT UNSIGNED DEFAULT 0;
	DECLARE mGens,fGens,pGens TEXT DEFAULT '';
	
	if @@session.max_sp_recursion_depth=0 THEN SET max_sp_recursion_depth=7; END IF;		
	if @@session.max_sp_recursion_depth=depth THEN LEAVE root; END IF;		
	
	SELECT COUNT(*) INTO cnt FROM rabbits WHERE r_id=rid;
	IF cnt!=0 THEN
		SELECT r_mother,r_father INTO mId,fId FROM rabbits WHERE r_id=rid;
	ELSE
		SELECT r_mother,r_father INTO mId,fId FROM dead WHERE r_id=rid;
	END IF;
	
	SELECT depth+1,depth+1 INTO mDpt,fDpt;
	
	IF mId!=0 THEN CALL FindRabGenoms(mId,mDpt,mGens); END IF;
	IF fId!=0 THEN CALL FindRabGenoms(fId,fDpt,fGens); END IF;
	SET mGens = Coalesce(mGens,'');
	SET fGens = Coalesce(fGens,'');
	SELECT IF(mDpt<=fDpt,mDpt,fDpt) INTO depth;
	
	IF mGens !='' AND fGens!='' THEN        
		SET pGens = Concat('{',mGens,',',fGens,'}');
	ELSEIF mGens !='' THEN
		SET pGens = Concat('{',mGens,'}');
	ELSEIF fGens !='' THEN
		SET pGens = Concat('{',fGens,'}');
	END IF;
	
	IF pGens!='' THEN      
		SELECT Concat(rid,':',pGens) INTO genoms;
	ELSE 
		SELECT Concat(rid) INTO genoms;
	END IF;
END|

drop FUNCTION if exists `GetRabGenoms`|
CREATE FUNCTION `GetRabGenoms`(rid INT UNSIGNED) RETURNS text CHARSET cp1251
BEGIN
	DECLARE res TEXT;
	DECLARE depth SMALLINT UNSIGNED DEFAULT 0;
	SET max_sp_recursion_depth = (SELECT Cast(o_value as SIGNED ) FROM options WHERE o_name='opt' AND o_subname='rab_gen_depth');
	CALL FindRabGenoms(rid,depth,res);
	RETURN res;
END|

drop procedure if exists `ParceGenoms`|
CREATE PROCEDURE `ParceGenoms`(rabGenoms TEXT,OUT mGens TEXT,OUT fGens TEXT)
root:BEGIN
	DECLARE i,lnt,serc SMALLINT DEFAULT 0;
	DECLARE sub VARCHAR(1);
	DECLARE ff,canp BOOLEAN DEFAULT false; #first find
	SET mGens = '';
	SET fGens = '';
	IF rabGenoms='' OR Locate('{',rabGenoms)=0 THEN LEAVE root; END IF;

	#убираем номер ID кроля, оставляем только его родителей
	SET rabGenoms = SubStr(rabGenoms,Locate('{',rabGenoms)+1);
    SET rabGenoms = SubStr(rabGenoms,1,Length(rabGenoms)-1);
	SET lnt = Length(rabGenoms);
	#проверка указаны ли деды
	SET i = Locate('{',rabGenoms);
	IF i>0 THEN
		SET i=1;
		#определяем указан один родитель или 2е
		wl:WHILE i<=lnt DO
			SET sub = Substr(rabGenoms,i,1);
			IF sub='{' THEN
				SET ff=true;
				SET serc=serc+1;
			ELSEIF sub='}' THEN
				SET serc=serc-1;
			END IF;
			IF (sub=',' AND NOT ff) THEN
				SET i=i-1;
				SET canp=true;
				LEAVE wl;
			END IF;
			IF ff AND serc=0 THEN
				SET canp=true;
				LEAVE wl;
			END IF;		
			SET i=i+1;
		END WHILE;
		IF canp THEN
			SET mGens=SubStr(rabGenoms,1,i);
			#определяем указан ли второй родитель
			SET sub=SubStr(rabGenoms,i+1,1);
			IF sub=',' THEN 
				SET fGens=SubStr(rabGenoms,i+2);
			END IF;
			LEAVE root;#return
		END IF;
	ELSE #если без дедов и прадедов
		SET i = Locate(',',rabGenoms);
		IF i>0 THEN 
			SET mGens = SubStr(rabGenoms,1,i-1);
			SET fGens = SubStr(rabGenoms,i+1);
		ELSE
			SET mGens = rabGenoms;
		END IF;	
	END IF;	
END|