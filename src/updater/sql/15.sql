UPDATE options SET o_value='15' WHERE o_name='db' AND o_subname='version';

ALTER TABLE  `rabbits` 
	ADD  `r_birthplace` INT NULL DEFAULT NULL COMMENT  'программа откуда экспортирован кролик' AFTER  `r_born`;
	
ALTER TABLE  `dead` 
	ADD  `r_birthplace` INT NULL DEFAULT NULL COMMENT  'программа откуда экспортирован кролик' AFTER  `r_born`;

DROP TABLE IF EXISTS `import`;	
	
RENAME TABLE `income` TO `import` ;
ALTER TABLE `import`  
	ADD `t_client` INT NULL DEFAULT NULL COMMENT 'id клиента из которой экспортирован, с которой привsезен кролик',  
	ADD `t_old_r_id` INT NULL DEFAULT NULL COMMENT 'id кролика в программе экспортера',
	ADD `t_file_guid` VARCHAR(40) NULL COMMENT  'guid файла экспорта';
	
CREATE TABLE IF NOT EXISTS `clients` (
  `c_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `c_org` varchar(45) NOT NULL COMMENT 'название организации',
  `c_address` varchar(100) NOT NULL,
  PRIMARY KEY (`c_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE IF NOT EXISTS `import_ascendants` (
  `r_id` int(10) unsigned NOT NULL,
  `r_mother` int(10) unsigned NOT NULL DEFAULT '0',
  `r_father` int(10) unsigned NOT NULL DEFAULT '0',
  `r_sex` enum('male','female','void') NOT NULL,
  `r_bon` VARCHAR(5) NOT NULL DEFAULT '10000',
  `r_name` int(10) unsigned NOT NULL DEFAULT '0',
  `r_surname` int(10) unsigned NOT NULL DEFAULT '0',
  `r_secname` int(10) unsigned NOT NULL DEFAULT '0',
  `r_breed` int(10) unsigned NOT NULL DEFAULT '0',
  `r_born` datetime DEFAULT NULL,
  `r_birthplace` int(11) unsigned DEFAULT NULL COMMENT 'программа откуда экспортирован кролик',
  UNIQUE KEY `u_r_id_birtplace` (`r_id`,`r_birthplace`),
  KEY `r_mother` (`r_mother`),
  KEY `r_father` (`r_father`),
  KEY `r_sex` (`r_sex`),
  KEY `r_name` (`r_name`),
  KEY `r_surname` (`r_surname`),
  KEY `r_secname` (`r_secname`),
  KEY `r_breed` (`r_breed`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES
(34,'Импорт кролика','$r ->$a ($t)'),
(35,'Импорт породы','$a ($t)'),
(36,'Импорт имени','$a ($t)');


#DELIMITER |

DROP FUNCTION IF EXISTS `ascname`|
CREATE FUNCTION `ascname`(rid INTEGER UNSIGNED,cid INTEGER UNSIGNED,sur INTEGER) RETURNS char(150)
BEGIN
  DECLARE n,sr,sc,sx CHAR(50);
  DECLARE res CHAR(150);
  IF(rid=0 OR rid IS NULL) THEN
    return '';
  END IF;
  SELECT
	(SELECT n_name FROM names WHERE n_id=r_name) name,
	(SELECT n_surname FROM names WHERE n_id=r_surname) surname,
	(SELECT n_surname FROM names WHERE n_id=r_secname) secname,
	r_sex INTO n,sr,sc,sx FROM import_ascendants WHERE r_id=rid and r_birthplace=cid;
  SET res='';
  IF (n IS NOT NULL) THEN
	SET res=n;
  END IF;
  IF (sur>0 AND NOT sr IS NULL) THEN
	IF (res='') THEN
		SET res=sr;
	ELSE
		SET res=CONCAT_WS(' ',res,sr);
	END IF;
    if (sx='female') then SET res=CONCAT(res,'а'); end if;
  END IF;
  IF (sur>1 AND NOT sc IS NULL) THEN
    SET res=CONCAT_WS('-',res,sc);
    if (sx='female') then SET res=CONCAT(res,'а'); end if;
  END IF;
  RETURN(res);
END |