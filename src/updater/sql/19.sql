ALTER TABLE `names` CHANGE COLUMN `n_use` `n_use` INT(10) UNSIGNED NULL DEFAULT NULL ;

UPDATE names SET n_use = null WHERE n_use not in (select r_id from rabbits);

ALTER TABLE `names` 
ADD CONSTRAINT `u_use`
  FOREIGN KEY (`n_use`)
  REFERENCES `rabbits` (`r_id`)
  ON DELETE SET NULL
  ON UPDATE CASCADE;
  
UPDATE options SET o_value = '19' WHERE o_name = 'db' AND o_subname = 'version';