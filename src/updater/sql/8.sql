UPDATE options SET o_value='8' WHERE o_name='db' AND o_subname='version';

DROP TABLE IF EXISTS `scaleprod`;
CREATE TABLE `scaleprod` (
  `s_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `s_date` DATETIME NOT NULL,
  `s_plu_id` INTEGER UNSIGNED NOT NULL,
  `s_plu_name` VARCHAR(56) NOT NULL DEFAULT '""',
   `s_tsell` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `s_tsumm` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `s_tweight` FLOAT UNSIGNED NOT NULL,
  `s_cleared` DATETIME NOT NULL,
  PRIMARY KEY (`s_id`)
) ENGINE = InnoDB;

#DELIMITER |

DROP PROCEDURE IF EXISTS addPLUSummary |
CREATE PROCEDURE addPLUSummary(prodid integer unsigned,prodname VARCHAR(56) , tsell integer unsigned, tsumm integer unsigned, tweight integer unsigned, cleared DateTime)
BEGIN
	DECLARE preSell,preSumm,preWeight INTEGER;
	SELECT s_tsell,s_tsumm,s_tweight INTO preSell,preSumm,preWeight FROM scaleprod WHERE s_plu_id=prodid ORDER BY s_id DESC LIMIT 1;
	IF(isnull(preSell) OR isnull(preSumm) OR isnull(preWeight)) THEN
		INSERT INTO scaleprod(s_date, s_plu_id, s_plu_name, s_tsell,s_tsumm,s_tweight,s_cleared) VALUES(NOW(),prodid, prodname, tsell, tsumm, tweight, cleared);
	END IF;
	IF (tsell<>preSell OR tsumm<>preSumm OR tweight<>preWeight)THEN
		INSERT INTO scaleprod(s_date,s_plu_id,s_plu_name,s_tsell,s_tsumm,s_tweight,s_cleared) VALUES(NOW(),prodid, prodname, tsell, tsumm, tweight, cleared);
	END IF;
END |

DROP FUNCTION IF EXISTS appendPLUSell |
CREATE FUNCTION appendPLUSell(id INTEGER UNSIGNED) RETURNS int(11)
BEGIN
	DECLARE preSell,nowSell,nowProdId INTEGER;
	DECLARE preClear,nowClear DateTime;
	SELECT s_tsell,s_cleared,s_plu_id INTO nowSell,nowClear,nowProdId FROM scaleprod WHERE s_id=id;
  IF (isnull(nowSell) OR isnull(nowProdId)) THEN
    return 0;
  END IF;
	SELECT s_tsell,s_cleared INTO preSell,preClear FROM scaleprod WHERE s_id<id AND s_plu_id=nowProdId ORDER BY s_id DESC LIMIT 1;
	IF(isnull(preClear) OR preClear<>nowClear) THEN
		return nowSell;
	ELSE
    return nowSell-preSell;
	END IF;
END |

DROP FUNCTION IF EXISTS appendPLUSumm |
CREATE FUNCTION appendPLUSumm(id INTEGER UNSIGNED) RETURNS int(11)
BEGIN
	DECLARE preSumm,nowSumm,nowProdId INTEGER;
	DECLARE preClear,nowClear DateTime;
	SELECT s_tsumm,s_cleared,s_plu_id INTO nowSumm,nowClear,nowProdId FROM scaleprod WHERE s_id=id;
  IF (isnull(nowSumm) OR isnull(nowProdId)) THEN
    return 0;
  END IF;
	SELECT s_tsumm,s_cleared INTO preSumm,preClear FROM scaleprod WHERE s_id<id AND s_plu_id=nowProdId ORDER BY s_id DESC LIMIT 1;
	IF(isnull(preClear) OR preClear<>nowClear) THEN
		return nowSumm;
	ELSE
    return nowSumm-preSumm;
	END IF;
END |

DROP FUNCTION IF EXISTS appendPLUWeight |
CREATE FUNCTION appendPLUWeight(id INTEGER UNSIGNED) RETURNS int(11)
BEGIN
	DECLARE preWeight,nowWeight,nowProdId INTEGER;
	DECLARE preClear,nowClear DateTime;
	SELECT s_tweight,s_cleared,s_plu_id INTO nowWeight,nowClear,nowProdId FROM scaleprod WHERE s_id=id;
  IF (isnull(nowWeight) OR isnull(nowProdId)) THEN
    return 0;
  END IF;
	SELECT s_tweight,s_cleared INTO preWeight,preClear FROM scaleprod WHERE s_id<id AND s_plu_id=nowProdId ORDER BY s_id DESC LIMIT 1;
	IF(isnull(preClear) OR preClear<>nowClear) THEN
		return nowWeight;
	ELSE
    return nowWeight-preWeight;
	END IF;
END |

DROP FUNCTION IF EXISTS mealCalculate |
CREATE FUNCTION mealCalculate(id INTEGER UNSIGNED) RETURNS float
BEGIN
	DECLARE days,i,a,d INTEGER;
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
	IF res=0 THEN
		return 0;
	END IF;
	return (amnt/res);
END |

DROP PROCEDURE IF EXISTS addMeal |
CREATE PROCEDURE addMeal(startDate DateTime,amount integer unsigned)
BEGIN
  DECLARE i,oldI,maxId INTEGER;
  DECLARE oldSD,oldED,yngSD,yngED DateTime;
  INSERT INTO meal(m_start_date,m_amount) VALUES(startDate,amount);#add new record
  SELECT m_id INTO maxId FROM meal ORDER BY m_start_date DESC LIMIT 1;
  SELECT m_id,m_start_date,m_end_date INTO i,yngSD,yngED FROM meal ORDER BY m_start_date ASC LIMIT 1;#id of later date
  WHILE (i<>maxId) DO
    SELECT m_id,m_start_date,m_end_date INTO oldI, oldSD,oldED FROM meal WHERE m_start_date>yngSD ORDER BY m_start_date ASC LIMIT 1;
	IF (isnull(yngED) OR yngED<>oldSD) THEN
		UPDATE meal SET m_end_date=oldSD WHERE m_id=i;
		UPDATE meal SET m_rate=mealCalculate(i) WHERE m_id=i;
	END IF;	
    SET i=oldI;
    SET yngSD=oldSD;
	SET yngED=oldED;
  END WHILE;
END |

#DELIMITER ;
