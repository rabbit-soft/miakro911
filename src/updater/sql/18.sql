DROP PROCEDURE IF EXISTS updateMeal;
DROP FUNCTION IF EXISTS mealCalculate;

ALTER TABLE  `dead` CHANGE  `r_born`  `r_born` DATE NULL DEFAULT NULL ;
ALTER TABLE  `dead` CHANGE  `d_date`  `d_date` DATE NULL DEFAULT NULL ;

DROP VIEW IF EXISTS allrabbits;
CREATE VIEW allrabbits AS
  (SELECT r_id,r_sex,r_bon,r_name,r_surname,r_secname, r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone, r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies, d_date, d_reason, d_notes FROM dead)
UNION
  (SELECT r_id,r_sex,r_bon,r_name,r_surname,r_secname, r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone, r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies, NULL, 0, '' FROM rabbits);
  
  
UPDATE options SET o_value = '18' WHERE o_name = 'db' AND o_subname = 'version';