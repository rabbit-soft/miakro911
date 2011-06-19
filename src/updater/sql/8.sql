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

#DELIMITER ;
