UPDATE options SET o_value='10' WHERE o_name='db' AND o_subname='version';

DROP TABLE IF EXISTS `vaccines`;
CREATE  TABLE `vaccines` (
  `v_id` INT UNSIGNED NOT NULL AUTO_INCREMENT ,
  `v_name` VARCHAR(45) NULL ,
  `v_duration` INT UNSIGNED NOT NULL COMMENT 'Продолжительность прививки в Днях' ,
  `v_age` INT UNSIGNED NOT NULL COMMENT 'Назначать с (дней)' ,
  `v_zootech` BIT NOT NULL DEFAULT 0 COMMENT 'Отображать в Зоотехплане',
  PRIMARY KEY (`v_id`) ,
  UNIQUE INDEX `v_name_UNIQUE` (`v_name` ASC) 
)ENGINE = InnoDB DEFAULT CHARACTER SET = utf8 COMMENT = 'Список имеющихся прививок';

DROP TABLE IF EXISTS `rab_vac`;
CREATE  TABLE `rab_vac` (
  `r_id` INT UNSIGNED NOT NULL ,
  `v_id` INT UNSIGNED NOT NULL COMMENT 'Тип прививки' ,
  `date` DATETIME NULL COMMENT 'Когда была сделана прививка',
  `unabled` BIT NOT NULL DEFAULT 0 COMMENT 'Отменена ли прививка'
)ENGINE = InnoDB DEFAULT CHARACTER SET = utf8 COMMENT = 'Какие прививки делались кролику';


SET @end_vac = (SELECT o_value FROM Options WHERE o_subname='vaccine_time');
SET @v_age = (SELECT o_value FROM Options WHERE o_subname='vacc');
#INSERT INTO vaccines(v_id,v_name,v_duration,v_age,v_zootech) VALUES(1,'Прививка',@end_vac,@v_age,1);
#INSERT INTO rab_vac(r_id,v_id,date) (SELECT r_id,1,r_vaccine_end - INTERVAL @end_vac day from rabbits);


ALTER TABLE rabbits DROP COLUMN `r_vaccine_end` ;
ALTER TABLE dead DROP COLUMN `r_vaccine_end` ;

#DELIMITER |

DROP PROCEDURE IF EXISTS killRabbitDate |
CREATE PROCEDURE killRabbitDate (rid INTEGER UNSIGNED,reason INTEGER UNSIGNED,notes TEXT,ddate DATETIME)
BEGIN
  INSERT INTO dead(d_date,d_reason,d_notes,r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,r_parent,r_father,r_mother)
SELECT ddate,reason,notes,r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,r_parent,r_father,r_mother
FROM rabbits WHERE r_id=rid;
DELETE FROM rabbits WHERE r_id=rid;
END |

#DELIMITER ;


