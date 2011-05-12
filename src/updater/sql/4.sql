UPDATE options SET o_value='4' WHERE o_name='db' AND o_subname='version';

UPDATE rabbits SET r_flags=SUBSTR(r_flags,1,5) WHERE r_flags LIKE '______';
UPDATE dead SET r_flags=SUBSTR(r_flags,1,5) WHERE r_flags LIKE '______';
INSERT INTO options (o_name,o_subname,o_uid,o_value) VALUES ('opt','vaccine_time',0,365);
INSERT INTO options (o_name,o_subname,o_uid,o_value) VALUES ('opt','candidate',0,120); 																		#+
ALTER TABLE rabbits ADD COLUMN r_vaccine_end DATETIME NULL;
ALTER TABLE dead ADD COLUMN r_vaccine_end DATETIME NULL;
UPDATE rabbits SET r_vaccine_end=NOW()+interval 365 day WHERE r_flags LIKE '__2__' OR r_flags LIKE '__3__';
UPDATE rabbits SET r_vaccine_end=NOW() WHERE r_flags LIKE '__0__';
UPDATE rabbits SET r_flags=CONCAT(SUBSTR(r_flags,1,1), SUBSTR(r_flags,2,1), '0', SUBSTR(r_flags,4,1), SUBSTR(r_flags,5,1)) where r_flags LIKE '__2__';
UPDATE rabbits SET r_flags=CONCAT(SUBSTR(r_flags,1,1), SUBSTR(r_flags,2,1), '1', SUBSTR(r_flags,4,1), SUBSTR(r_flags,5,1)) where r_flags LIKE '__3__';
UPDATE rabbits SET r_last_fuck_okrol = null WHERE r_sex='male' AND r_status=0;																				#+

CREATE TEMPORARY TABLE aaa SELECT r_id, r_lost_babies, r_overall_babies FROM rabbits WHERE r_sex='female' AND r_overall_babies < r_lost_babies;				#+
UPDATE rabbits r, aaa SET r.r_overall_babies = aaa.r_lost_babies, r.r_lost_babies=aaa.r_overall_babies WHERE r.r_id = aaa.r_id;								#+

#DELIMITER |
DROP PROCEDURE IF EXISTS `killRabbitDate` |
CREATE PROCEDURE `killRabbitDate`(rid INTEGER UNSIGNED,reason INTEGER UNSIGNED,notes TEXT,ddate DATETIME)
BEGIN
  INSERT INTO dead(d_date,d_reason,d_notes,r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,r_parent,r_father,r_mother,r_vaccine_end)
SELECT ddate,reason,notes,r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,r_parent,r_father,r_mother,r_vaccine_end
FROM rabbits WHERE r_id=rid;
DELETE FROM rabbits WHERE r_id=rid;
END |
#DELIMITER ;

