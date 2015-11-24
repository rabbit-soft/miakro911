ALTER TABLE  `buildings` CHANGE  `b_farm`  `b_farm` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
UPDATE buildings SET b_farm = NULL WHERE b_farm = 0;

ALTER TABLE  `buildings` 
	DROP INDEX  `b_farm`, 
	ADD UNIQUE  `b_farm` (`b_farm`) COMMENT  '';

ALTER TABLE  `buildings` 
	ADD FOREIGN KEY (  `b_farm` ) REFERENCES  `minifarms` (`m_id`) 
		ON DELETE SET NULL 
		ON UPDATE CASCADE;


UPDATE minifarms SET m_lower = NULL WHERE m_lower = 0;

ALTER TABLE `minifarms` 
	ADD FOREIGN KEY (`m_upper`) REFERENCES  `tiers` (`t_id`) 
		ON DELETE SET NULL 
		ON UPDATE CASCADE ;

ALTER TABLE `minifarms` 
	ADD FOREIGN KEY (`m_lower`) REFERENCES  `tiers` (`t_id`) 
		ON DELETE SET NULL 
		ON UPDATE CASCADE;


ALTER TABLE  `logs` 
	ADD FOREIGN KEY (`l_type`) REFERENCES `logtypes` (`l_type`) 
		ON DELETE CASCADE 
		ON UPDATE CASCADE;




ALTER TABLE  `rabbits` ENGINE = INNODB;

ALTER TABLE  `rabbits` CHANGE  `r_parent`  `r_parent` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
UPDATE rabbits SET r_parent = NULL WHERE r_parent =0

ALTER TABLE  `rabbits` CHANGE  `r_mother`  `r_mother` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
ALTER TABLE  `rabbits` CHANGE  `r_father`  `r_father` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;

UPDATE rabbits SET r_mother = NULL WHERE r_mother = 0;
UPDATE rabbits SET r_father = NULL WHERE r_father = 0;

ALTER TABLE  `rabbits` ADD FOREIGN KEY (`r_parent`) REFERENCES  `rabbits` (`r_id`) 
	ON DELETE SET NULL 
	ON UPDATE CASCADE;

	
ALTER TABLE  `rabbits` CHANGE  `r_farm`  `r_farm` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
UPDATE rabbits SET r_farm = NULL WHERE r_farm =0;

ALTER TABLE  `rabbits` ADD FOREIGN KEY (`r_farm`) REFERENCES  `minifarms` (`m_id`) 
	ON DELETE SET NULL 
	ON UPDATE CASCADE ;
	
ALTER TABLE  `rabbits` CHANGE  `r_tier`  `r_tier` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
UPDATE rabbits SET r_tier=null WHERE r_tier=0;

ALTER TABLE  `rabbits` ADD FOREIGN KEY (`r_tier`) REFERENCES  `tiers` (`t_id`) 
	ON DELETE SET NULL 
	ON UPDATE CASCADE ;
	
ALTER TABLE  `rabbits` ADD FOREIGN KEY (`r_breed`) REFERENCES  `breeds` (`b_id`) 
	ON DELETE RESTRICT 
	ON UPDATE CASCADE ;
	
ALTER TABLE  `rabbits` CHANGE  `r_name`  `r_name` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
UPDATE rabbits SET r_name = NULL WHERE r_name =0;

ALTER TABLE  `rabbits` ADD FOREIGN KEY (`r_name`) REFERENCES  `names` (`n_id`) 
	ON DELETE RESTRICT 
	ON UPDATE CASCADE ;
	
ALTER TABLE  `rabbits` CHANGE  `r_surname`  `r_surname` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
UPDATE rabbits SET r_surname = NULL WHERE r_surname =0;
ALTER TABLE  `rabbits` ADD FOREIGN KEY (`r_surname`) REFERENCES  `names` (`n_id`) 
	ON DELETE RESTRICT 
	ON UPDATE CASCADE ;
	
ALTER TABLE  `rabbits` CHANGE  `r_secname`  `r_secname` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
UPDATE rabbits SET r_secname = NULL WHERE r_secname =0;
ALTER TABLE  `rabbits` ADD FOREIGN KEY (`r_secname`) REFERENCES  `names` (`n_id`) 
	ON DELETE RESTRICT 
	ON UPDATE CASCADE ;


UPDATE options SET o_value = '17' WHERE o_name = 'db' AND o_subname = 'version';