
UPDATE options SET o_value='2' WHERE o_name='db' AND o_subname='version';
ALTER TABLE options MODIFY o_value TEXT NULL;
ALTER TABLE filters MODIFY f_filter TEXT NULL;
ALTER TABLE tiers MODIFY t_notes TEXT NULL;
ALTER TABLE rabbits MODIFY r_notes TEXT NULL;
ALTER TABLE rabbits MODIFY r_genesis INT(10) UNSIGNED NOT NULL DEFAULT 0;
ALTER TABLE fucks MODIFY f_notes TEXT NULL;
ALTER TABLE genesis MODIFY g_notes TEXT NULL;
ALTER TABLE dead MODIFY d_notes TEXT NULL;
ALTER TABLE dead MODIFY r_notes TEXT NULL;
ALTER TABLE logtypes MODIFY l_params TEXT NULL;
ALTER TABLE logs MODIFY l_param TEXT NULL;

DELETE FROM options WHERE o_subname='rablist' OR o_subname='zoolist';

ALTER TABLE breeds add b_color VARCHAR(100) NOT NULL default "White";

INSERT INTO options(o_name,o_subname,o_value) VALUES ('opt','short_zoo',1);


#DELIMITER |

DROP FUNCTION IF EXISTS inBuilding |
CREATE FUNCTION inBuilding(building INTEGER UNSIGNED,farm INTEGER UNSIGNED) RETURNS BOOL
BEGIN
  DECLARE cid INTEGER UNSIGNED;
  IF (farm=0) THEN return 0; END IF;
  SELECT b_id INTO cid FROM buildings WHERE b_farm=farm;
  while cid<>0 DO
    SELECT b_parent INTO cid FROM buildings WHERE b_id=cid;
    IF cid=building THEN return 1; END IF;
  end while;
  return 0;
END |

#DELIMITER ;