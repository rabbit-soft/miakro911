UPDATE options SET o_value='12' WHERE o_name='db' AND o_subname='version';

ALTER TABLE `vaccines` CHANGE `v_id`  `v_id` TINYINT NOT NULL AUTO_INCREMENT;
ALTER TABLE `rab_vac` CHANGE  `v_id`  `v_id` TINYINT NOT NULL COMMENT  'Тип прививки';

INSERT INTO `vaccines`(v_id,v_name,v_do_after,v_duration,v_age,v_zootech,v_do_times) VALUES(-1,'Стимуляция самки',0,3,2,0,0);

ALTER TABLE `fucks` CHANGE `f_type`  `f_type` ENUM('sluchka', 'vyazka', 'kuk', 'syntetic') NOT NULL DEFAULT 'vyazka';
ALTER TABLE `rabbits` CHANGE `r_event`  `r_event` ENUM('none','sluchka','vyazka','kuk','syntetic') NULL DEFAULT NULL;

UPDATE logtypes SET l_params='$r   + $R ($t)' WHERE l_type=5; #вязка