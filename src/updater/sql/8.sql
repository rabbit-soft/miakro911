UPDATE options SET o_value='8' WHERE o_name='db' AND o_subname='version';

ALTER TABLE `meal` ADD COLUMN `m_type` ENUM('in','out') NOT NULL DEFAULT 'in' AFTER `m_id`;

#DELIMITER |

DROP FUNCTION IF EXISTS mealCalculate |
CREATE FUNCTION mealCalculate(id INTEGER UNSIGNED) RETURNS float
BEGIN
	DECLARE days,i,a,d,sell INTEGER;
	DECLARE res,amnt FLOAT;
	DECLARE sd,ed DateTime;
	SELECT m_start_date, m_end_date INTO sd,ed FROM meal WHERE m_id=id;
	IF(isnull(sd) OR isnull(ed)) THEN
		return 0;
	END IF;
	SELECT to_days(m_end_date)-to_days(m_start_date) INTO days FROM meal WHERE m_id=id;
	IF(days = 0) THEN
		return 0;
	END IF;
	SET i=1; #in income day, meal is not eaten
	SET res=0;
	
	DROP TEMPORARY TABLE IF EXISTS deads;
	CREATE TEMPORARY TABLE deads AS
		SELECT r_group,r_born,d_date FROM dead    WHERE (AddDate(r_born,18)<=sd OR AddDate(r_born,18)<=ed ) AND d_date>=sd ORDER by r_born;

	DROP TEMPORARY TABLE IF EXISTS alives;
	CREATE TEMPORARY TABLE alives AS
		SELECT r_group,r_born  	     FROM rabbits WHERE (AddDate(r_born,18)<=sd OR AddDate(r_born,18)<=ed ) 	 	    ORDER by r_born;
		
	SELECT (select count(*) from deads), (select count(*) from alives) INTO a,d;

	IF a=0 AND d=0 THEN
		return 0;
	END IF;	
		
	WHILE i<=days DO
		SELECT COALESCE(SUM(r_group),0) INTO d FROM deads  WHERE (to_days(AddDate(sd,i))-to_days(r_born))>=18 AND d_date>=AddDate(sd,i);
		SELECT COALESCE(SUM(r_group),0) INTO a FROM alives WHERE (to_days(AddDate(sd,i))-to_days(r_born))>=18;
		SET res=res+d+a;
		SET i=i+1;
	END WHILE;
	SELECT m_amount INTO amnt FROM meal WHERE m_id=id;
	SELECT COALESCE(sum(m_amount),0) INTO sell FROM meal WHERE m_type='out' AND m_start_date BETWEEN sd AND ed;
	SET amnt=amnt-sell;
	IF (amnt<=0) THEN
		return 0;
	END IF;
	IF res=0 THEN
		return 0;
	END IF;
	return (amnt/res);
END |

DROP PROCEDURE IF EXISTS updateMeal |
CREATE PROCEDURE updateMeal()
root:BEGIN
  DECLARE i,oldI,maxId,sell INTEGER;
  DECLARE yngRate FLOAT;
  DECLARE oldSD,oldED,yngSD,yngED DateTime;
  SELECT COALESCE(m_id,0) INTO maxId FROM meal WHERE m_type='in' ORDER BY m_start_date DESC LIMIT 1;
  IF (maxId=0) THEN
	LEAVE root;
  END IF;
  SELECT m_id,m_start_date,m_end_date,m_rate INTO i,yngSD,yngED,yngRate FROM meal WHERE m_type='in' ORDER BY m_start_date ASC LIMIT 1;#id of later date
  WHILE (i<>maxId) DO
    SELECT m_id,m_start_date,COALESCE(m_end_date,'9999-12-31') INTO oldI, oldSD,oldED FROM meal WHERE m_type='in' AND m_start_date>yngSD ORDER BY m_start_date ASC LIMIT 1;
	SELECT COALESCE(sum(m_amount),0) INTO sell FROM meal WHERE m_type='out' AND m_start_date BETWEEN yngSD AND oldED;
	IF (isnull(yngED) OR yngED<>oldSD OR sell<>0 OR isnull(yngRate)) THEN		
		UPDATE meal SET m_end_date=oldSD WHERE m_id=i;
		UPDATE meal SET m_rate=mealCalculate(i) WHERE m_id=i;
	END IF;	
    SET i=oldI;
    SET yngSD=oldSD;
	SET yngED=oldED;
  END WHILE;
END |

#DELIMITER ;
