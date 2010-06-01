
UPDATE options SET o_value='3' WHERE o_name='db' AND o_subname='version';

#DELIMITER |

#DELIMITER ;
ALTER TABLE `breeds` MODIFY COLUMN `b_short_name` VARCHAR(10);
ALTER TABLE `rabbits` MODIFY COLUMN `r_flags` VARCHAR(10)  NOT NULL DEFAULT '00000';
ALTER TABLE `dead` MODIFY COLUMN `r_flags` VARCHAR(10) NOT NULL DEFAULT '00000';
