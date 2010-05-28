
UPDATE options SET o_value='3' WHERE o_name='db' AND o_subname='version';

#DELIMITER |

#DELIMITER ;
ALTER TABLE `kroliki`.`breeds` MODIFY COLUMN `b_short_name` VARCHAR(10);