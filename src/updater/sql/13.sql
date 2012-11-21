UPDATE options SET o_value='13' WHERE o_name='db' AND o_subname='version';

INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(26,'изменение Даты рождения','$r ($t)');
INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(27,'изменение Породы','$r ($t)');
INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(28,'изменение Количества','$r ($t)');
INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(29,'изменение Фамилии по Матери','$r ($t)');
INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(30,'изменение Фамилии по Отцу','$r ($t)');
INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(30,'изменение Зоны прибытия','$r ($t)');
INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(31,'изменение Рейтинга','$r ($t)');
INSERT INTO `logtypes`(`l_type`,`l_name`,`l_params`) VALUES(30,'изменение Статуса','$r ($t)');