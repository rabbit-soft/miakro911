UPDATE options SET o_value='12' WHERE o_name='db' AND o_subname='version';

ALTER TABLE `vaccines` CHANGE `v_id`  `v_id` TINYINT NOT NULL AUTO_INCREMENT;
ALTER TABLE `rab_vac` CHANGE  `v_id`  `v_id` TINYINT NOT NULL COMMENT  'Тип прививки';

INSERT INTO `vaccines`(v_id,v_name,v_do_after,v_duration,v_age,v_zootech,v_do_times) VALUES(-1,'Стимуляция самки',0,3,2,0,0);

ALTER TABLE `fucks` CHANGE `f_type`  `f_type` ENUM('sluchka', 'vyazka', 'kuk', 'syntetic') NOT NULL DEFAULT 'vyazka';
ALTER TABLE `rabbits` CHANGE `r_event`  `r_event` ENUM('none','sluchka','vyazka','kuk','syntetic') NULL DEFAULT NULL;

UPDATE logtypes SET l_params='$r   + $R ($t)' WHERE l_type=5; #вязка

CREATE TABLE logs_arch(
	l_id INTEGER UNSIGNED NOT NULL PRIMARY KEY,
	l_date DATETIME NOT NULL,
	l_type INTEGER UNSIGNED NOT NULL,
	l_user INTEGER UNSIGNED NOT NULL,
	l_rabbit INTEGER UNSIGNED,
	l_address VARCHAR(50) NOT NULL DEFAULT '',
	l_rabbit2 INTEGER UNSIGNED,
	l_address2 VARCHAR(50) NOT NULL DEFAULT '',
	l_param TEXT,
	KEY(l_rabbit),
	KEY(l_rabbit2),
	KEY(l_address),
	KEY(l_address2),
	KEY(l_date),
	KEY(l_type)
);

INSERT INTO logs_arch(l_id,l_date,l_type,l_user,l_rabbit,l_address,l_rabbit2,l_address2,l_param)
  	(SELECT l_id,l_date,l_type,l_user,l_rabbit,l_address,l_rabbit2,l_address2,l_param FROM logs WHERE l_date<Date_Add(NOW(), INTERVAL -6 month));

DELETE FROM logs WHERE l_date<Date_Add(NOW(), INTERVAL -6 month);