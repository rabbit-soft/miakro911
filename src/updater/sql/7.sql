UPDATE options SET o_value='7' WHERE o_name='db' AND o_subname='version';

CREATE TABLE `kroliki`.`products` (
  `p_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `p_name` VARCHAR(45) NOT NULL DEFAULT '' COMMENT 'название продукции',
  `p_unit` VARCHAR(30) NOT NULL DEFAULT '' 'единица измерения',
  `p_image` BLOB COMMENT 'изображение',
  `p_imgsize` INTEGER UNSIGNED  COMMENT 'размер изображения',
  PRIMARY KEY (`p_id`)
)ENGINE = InnoDB COMMENT = 'продукция получаемая из кролика';

ALTER TABLE `kroliki`.`users` MODIFY COLUMN `u_group` ENUM('worker','admin','zootech','butcher') NOT NULL DEFAULT 'admin';

CREATE TABLE `kroliki`.`butcher` (
  `b_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `b_date` DATETIME NOT NULL COMMENT 'дата взвешивания',
  `b_prodtype` INTEGER UNSIGNED NOT NULL COMMENT 'тип продукта',
  `b_amount` FLOAT UNSIGNED NOT NULL COMMENT 'количество ГП',
  `b_user` INTEGER UNSIGNED NOT NULL COMMENT 'пользователь',
  PRIMARY KEY (`b_id`)
)
ENGINE = InnoDB
COMMENT = 'готовая продукция';

INSERT INTo logtypes (l_name,l_params) VALUES("Изменение причины списания","$r");

CREATE TABLE `kroliki`.`meal` (
  `m_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `m_start_date` DATETIME NOT NULL COMMENT 'дата завоза комбикорма',
  `m_end_date` DATETIME NULL COMMENT 'дата когда комбикорм закончился',
  `m_amount` INTEGER UNSIGNED NOT NULL DEFAULT 0 COMMENT 'в КилоГраммах',
  `m_rate` FLOAT UNSIGNED NOT NULL COMMENT 'кг комбикорма съедает кролик в день',
  PRIMARY KEY (`m_id`)
)
ENGINE = InnoDB COMMENT = 'Таблица расчета кормов';

#DELIMITER |
DROP FUNCTION IF EXISTS mealCalculate |
CREATE FUNCTION mealCalculate (id INTEGER UNSIGNED) RETURNS FLOAT
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
	SET i=1; #в день завоза новый корм не расходуется
	SET res=0;
	
	DROP TEMPORARY TABLE IF EXISTS deads;	
	CREATE TEMPORARY TABLE deads AS
		SELECT r_group,r_born,d_date FROM dead    WHERE (AddDate(r_born,18)<=sd OR AddDate(r_born,18)<=ed ) AND d_date>=sd ORDER by r_born;
	
	DROP TEMPORARY TABLE IF EXISTS alives;
	CREATE TEMPORARY TABLE alives AS
		SELECT r_group,r_born  	     FROM rabbits WHERE (AddDate(r_born,18)<=sd OR AddDate(r_born,18)<=ed ) 	 	    ORDER by r_born;
		
	WHILE i<days DO
		SELECT COALESCE(SUM(r_group),0) INTO d FROM deads  WHERE (to_days(AddDate(sd,i))-to_days(r_born))>=18 AND d_date>=AddDate(sd,i);
		SELECT COALESCE(SUM(r_group),0) INTO a FROM alives WHERE (to_days(AddDate(sd,i))-to_days(r_born))>=18;
		SET res=res+d+a;
		SET i=i+1;
	END WHILE;
	SELECT m_amount INTO amnt FROM meal WHERE m_id=id;
	return (amnt/res);
END|
#DELIMITER ;

