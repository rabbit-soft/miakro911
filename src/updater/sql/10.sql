UPDATE options SET o_value='10' WHERE o_name='db' AND o_subname='version';

DROP TABLE IF EXISTS `vaccines`;
CREATE  TABLE `vaccines` (
  `v_id` INT UNSIGNED NOT NULL AUTO_INCREMENT ,
  `v_name` VARCHAR(45) NULL ,
  `v_duration` INT UNSIGNED NOT NULL COMMENT 'Продолжительность прививки в Днях' ,
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


INSERT INTO vaccines(v_id,v_name,v_duration,v_zootech) VALUES(1,'Прививка',356,1);

INSERT INTO rab_vac(r_id,v_id,date) (SELECT r_id,1,r_vaccine_end - INTERVAL 365 day from rabbits);

ALTER TABLE rabbits DROP COLUMN `r_vaccine_end` ;
ALTER TABLE dead DROP COLUMN `r_vaccine_end` ;


