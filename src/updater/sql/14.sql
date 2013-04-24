UPDATE options SET o_value='14' WHERE o_name='db' AND o_subname='version';

INSERT INTO `options`(o_name,o_subname,o_value) VALUES('opt','rab_gen_depth','7');

#DELIMITER |

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
CREATE FUNCTION `GetRabGenoms`(rid INT UNSIGNED) RETURNS text CHARSET utf8
BEGIN
	DECLARE res TEXT;
	DECLARE depth SMALLINT UNSIGNED DEFAULT 0;
	SET max_sp_recursion_depth = (SELECT Cast(o_value as SIGNED ) FROM options WHERE o_name='opt' AND o_subname='rab_gen_depth');
	CALL FindRabGenoms(rid,depth,res);
	RETURN res;
END|