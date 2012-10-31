UPDATE options SET o_value='11' WHERE o_name='db' AND o_subname='version';

ALTER TABLE `vaccines` ADD `v_do_after` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'делать после рождения(0) или после прививки с ID' AFTER `v_name`;
ALTER TABLE `rab_vac` CHANGE  `date`  `date` DATE NULL DEFAULT NULL COMMENT 'Когда была сделана прививка';

INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(25,'Прививка','$r ($t)');