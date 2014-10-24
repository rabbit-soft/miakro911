DROP TABLE IF EXISTS users;
CREATE TABLE users(
	u_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	u_name VARCHAR(50),
	u_password VARCHAR(50),
	u_group enum('worker','admin','zootech','butcher') NOT NULL DEFAULT 'admin',
	u_deleted TINYINT(1) NOT NULL DEFAULT '0',
	KEY(u_name)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS options;
CREATE TABLE options(
	o_name VARCHAR(30) NOT NULL default '',
	o_subname VARCHAR(30) NOT NULL default '',
	o_uid INTEGER UNSIGNED NOT NULL default 0,
	o_value TEXT ,
	KEY(o_name,o_subname),
	KEY(o_uid)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS filters;
CREATE TABLE filters(
	f_type VARCHAR(30) NOT NULL,
	f_name VARCHAR(30) NOT NULL,
	f_filter TEXT,
	KEY(f_type),
	KEY(f_name)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS breeds;
CREATE TABLE breeds(
	b_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	b_name VARCHAR(50) NOT NULL,
	b_short_name VARCHAR(20) NOT NULL,
	b_color VARCHAR(100) NOT NULL default "White"
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS names;
CREATE TABLE names(
	n_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	n_sex ENUM('male','female') NOT NULL, 
	n_name VARCHAR(50) NOT NULL,
	n_surname VARCHAR(50) NOT NULL,
	n_use INTEGER UNSIGNED NOT NULL default 0,
	n_block_date DATETIME DEFAULT NULL,
	KEY(n_sex),
	UNIQUE(n_name,n_sex),
	KEY(n_use),
	KEY(n_block_date)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS tiers;
CREATE TABLE tiers(
	t_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	t_type ENUM('none','female','dfemale','complex','jurta','quarta','vertep','barin','cabin') NOT NULL,
	t_repair BOOL NOT NULL default 0,
	t_notes TEXT,
	t_busy1 INTEGER UNSIGNED NULL default 0,
	t_busy2 INTEGER UNSIGNED NULL default 0,
	t_busy3 INTEGER UNSIGNED NULL default 0,
	t_busy4 INTEGER UNSIGNED NULL default 0,
	t_delims VARCHAR(3) NOT NULL default '000',
	t_heater VARCHAR(2) NOT NULL default '00',
	t_nest VARCHAR(2) NOT NULL default '00',
	KEY(t_type),
	KEY(t_repair),
	KEY(t_busy1),
	KEY(t_busy2),
	KEY(t_busy3),
	KEY(t_busy4),
	KEY(t_heater),
	KEY(t_nest)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS minifarms;
CREATE TABLE minifarms(
	m_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	m_upper INTEGER UNSIGNED NULL DEFAULT NULL,
	m_lower INTEGER UNSIGNED NULL DEFAULT NULL,
	KEY(m_upper),
	KEY(m_lower)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS buildings;
CREATE TABLE buildings(
	b_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	b_name VARCHAR(50),
	b_parent INTEGER UNSIGNED NOT NULL DEFAULT 0,
	b_level INTEGER UNSIGNED NOT NULL DEFAULT 0,
	b_farm INTEGER UNSIGNED NOT NULL DEFAULT 0,
	KEY(b_name),
	KEY(b_parent),
	KEY(b_level),
	KEY(b_farm)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS zones;
CREATE TABLE zones(
	z_id INTEGER UNSIGNED NOT NULL PRIMARY KEY,
	z_name VARCHAR(50) NOT NULL,
	z_short_name VARCHAR(20) NOT NULL
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS rabbits;
CREATE TABLE rabbits(
	r_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	r_parent INTEGER UNSIGNED NOT NULL default 0, #group devision
	r_mother INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_father INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_sex ENUM('male','female','void') NOT NULL,
	r_bon VARCHAR(5) NOT NULL DEFAULT '10000',
	r_name INTEGER UNSIGNED NOT NULL default 0,
	r_surname INTEGER UNSIGNED NOT NULL default 0,
	r_secname INTEGER UNSIGNED NOT NULL default 0,
	r_notes TEXT,
	r_okrol INTEGER UNSIGNED NOT NULL default 0,
	r_farm INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_tier INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_tier_id TINYINT UNSIGNED NOT NULL DEFAULT 0,
	r_area INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_rate INTEGER NOT NULL default 0,
	r_group INTEGER UNSIGNED NOT NULL DEFAULT 1,
	r_breed INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_flags VARCHAR(10) NOT NULL DEFAULT '00000' COMMENT '[0]Готовая Продукция [1]Готов к реализации [2]Брак [3]Не куковать [4]Потеря лактации',
	r_zone INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_born DATETIME,
	r_birthplace INT NULL DEFAULT NULL COMMENT  'программа откуда экспортирован кролик',
	r_genesis INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_status TINYINT UNSIGNED NOT NULL DEFAULT 0,   #boy-status/girl-borns
	r_last_fuck_okrol DATETIME,
	r_event ENUM('none','sluchka','vyazka','kuk','syntetic'),
	r_event_date DATETIME,
	r_lost_babies INTEGER UNSIGNED,
	r_overall_babies INTEGER UNSIGNED,
	KEY(r_parent),
	KEY(r_mother),
	KEY(r_father),
	KEY(r_sex),
	KEY(r_name),KEY(r_surname),KEY(r_secname),
	KEY(r_farm),KEY(r_tier),
	KEY(r_group),
	KEY(r_breed),
	KEY(r_zone),
	KEY(r_status),
	KEY(r_born)
) ENGINE = MyISAM DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS fucks;
CREATE TABLE fucks(
	f_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	f_rabid INTEGER UNSIGNED NOT NULL,
	f_date DATETIME DEFAULT NULL,
	f_type enum('sluchka','vyazka','kuk','syntetic') NOT NULL default 'vyazka',
	f_partner INTEGER UNSIGNED NOT NULL,
	f_times INTEGER UNSIGNED NOT NULL DEFAULT 1,
	f_state enum('sukrol','proholost','okrol') NOT NULL default 'okrol',
	f_okrol INTEGER UNSIGNED NOT NULL default 0,
	f_end_date DATETIME DEFAULT NULL,
	f_children INTEGER UNSIGNED NOT NULL DEFAULT 0,
	f_dead INTEGER UNSIGNED NOT NULL DEFAULT 0,
	f_killed INTEGER UNSIGNED NOT NULL DEFAULT 0,
	f_added INTEGER NOT NULL DEFAULT 0,
	f_last BOOL NOT NULL DEFAULT 0,
	f_worker INTEGER UNSIGNED NOT NULL DEFAULT 0,
	f_notes TEXT,
	KEY(f_date),
	KEY(f_state),
	KEY(f_type),
	KEY(f_children),
	KEY(f_dead)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS genesis;
CREATE TABLE genesis(
	g_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	g_notes TEXT,
	g_key VARCHAR(50),
	KEY(g_id),
	KEY(g_key)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS genoms;
CREATE TABLE genoms(
	g_id INTEGER UNSIGNED NOT NULL,
	g_genom INTEGER UNSIGNED NOT NULL,
	KEY(g_id),
	KEY(g_genom),
	UNIQUE(g_id,g_genom)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS weights;
CREATE TABLE weights(
	w_rabid INTEGER UNSIGNED NOT NULL,
	w_date DATETIME NOT NULL,
	w_weight INTEGER UNSIGNED NOT NULL,
	KEY(w_rabid)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS deadreasons;
CREATE TABLE deadreasons(
	d_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	d_name VARCHAR(50) NOT NULL,
	d_rate INTEGER NOT NULL default 0
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS dead;
CREATE TABLE dead(
	d_date DATETIME NOT NULL,
	d_reason INTEGER UNSIGNED NOT NULL DEFAULT 0,
	d_notes TEXT,
	r_id INTEGER UNSIGNED NOT NULL PRIMARY KEY,
	r_parent INTEGER UNSIGNED NOT NULL default 0,
	r_father INTEGER UNSIGNED NOT NULL default 0,
	r_mother INTEGER UNSIGNED NOT NULL default 0,
	r_sex ENUM('male','female','void') NOT NULL,
	r_bon VARCHAR(5) NOT NULL DEFAULT '10000',
	r_name INTEGER UNSIGNED NOT NULL default 0,
	r_surname INTEGER UNSIGNED NOT NULL default 0,
	r_secname INTEGER UNSIGNED NOT NULL default 0,
	r_notes TEXT,
	r_okrol INTEGER UNSIGNED NOT NULL default 0,
	r_farm INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_tier INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_tier_id TINYINT UNSIGNED NOT NULL DEFAULT 0,
	r_area INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_rate INTEGER NOT NULL default 0,
	r_group INTEGER UNSIGNED NOT NULL DEFAULT 1,
	r_breed INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_flags VARCHAR(10) NOT NULL DEFAULT '00000',
	r_zone INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_born DATETIME,
	r_birthplace INT NULL DEFAULT NULL COMMENT  'программа откуда экспортирован кролик',
	r_genesis INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_status TINYINT UNSIGNED NOT NULL DEFAULT 0,
	r_last_fuck_okrol DATETIME,
	r_lost_babies INTEGER UNSIGNED,
	r_overall_babies INTEGER UNSIGNED,
	UNIQUE(r_id),
	KEY(r_parent),
	KEY(r_sex),
	KEY(r_name),KEY(r_surname),KEY(r_secname),
	KEY(r_farm),KEY(r_tier),
	KEY(r_group),
	KEY(r_breed),
	KEY(r_zone),
	KEY(r_status),
	KEY(r_born),
	KEY(d_date),
	KEY(d_reason)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS logtypes;
CREATE TABLE logtypes(
	l_type INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	l_name VARCHAR(30) NOT NULL,
	l_params TEXT,
	KEY(l_name)
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS logs;
CREATE TABLE logs(
	l_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
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
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS logs_arch;
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
) DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS import;
CREATE TABLE `import` (
  `t_rab_id` INTEGER UNSIGNED NOT NULL,
  `t_date` DATETIME NOT NULL,
  `t_count` INTEGER UNSIGNED,
  `t_client` INT NULL DEFAULT NULL COMMENT 'id клиента из которой экспортирован, с которой привsезен кролик',  
  `t_old_r_id` INT NULL DEFAULT NULL COMMENT 'id кролика в программе экспортера',
  `t_file_guid` VARCHAR(40) NULL COMMENT  'guid файла экспорта',
  PRIMARY KEY (`t_rab_id`)
) ENGINE = InnoDB  DEFAULT CHARSET=utf8 COMMENT = 'Таблица привозов';

DROP TABLE IF EXISTS products;
CREATE TABLE products (
  p_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  p_name VARCHAR(45) NOT NULL DEFAULT '' COMMENT 'название продукции',
  p_unit VARCHAR(30) NOT NULL DEFAULT '' 'единица измерения',
  p_image BLOB COMMENT 'изображение',
  p_imgsize INTEGER UNSIGNED  COMMENT 'размер изображения',
  PRIMARY KEY (p_id)
)ENGINE = InnoDB  DEFAULT CHARSET=utf8 COMMENT = 'продукция получаемая из кролика';

DROP TABLE IF EXISTS butcher;
CREATE TABLE butcher (
  b_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  b_date DATETIME NOT NULL COMMENT 'дата взвешивания',
  b_prodtype INTEGER UNSIGNED NOT NULL COMMENT 'тип продукта',
  b_amount FLOAT UNSIGNED NOT NULL COMMENT 'количество ГП',
  b_user INTEGER UNSIGNED NOT NULL COMMENT 'пользователь',
  PRIMARY KEY (b_id)
)ENGINE = InnoDB  DEFAULT CHARSET=utf8 COMMENT = 'готовая продукция';

DROP TABLE IF EXISTS meal;
CREATE TABLE meal (
  m_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  m_type ENUM('in','out') NOT NULL DEFAULT 'in',
  m_start_date DATETIME NOT NULL COMMENT 'дата завоза комбикорма', 
  m_end_date DATETIME NULL COMMENT 'дата когда комбикорм закончился',
  m_amount INTEGER UNSIGNED NOT NULL DEFAULT 0 COMMENT 'в КилоГраммах',
  m_rate FLOAT UNSIGNED NULL COMMENT 'кг комбикорма съедает кролик в день',
  PRIMARY KEY (m_id)
)ENGINE = InnoDB  DEFAULT CHARSET=utf8 COMMENT = 'Таблица расчета кормов';

DROP TABLE IF EXISTS `vaccines`;
CREATE  TABLE `vaccines` (
  `v_id` TINYINT NOT NULL AUTO_INCREMENT ,
  `v_name` VARCHAR(45) NULL ,
  `v_do_after` INT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'делать после рождения(0) или после прививки с ID',
  `v_duration` INT UNSIGNED NOT NULL COMMENT 'Продолжительность прививки в Днях' ,
  `v_age` INT UNSIGNED NOT NULL DEFAULT 45 COMMENT 'Назначать с (дней)' ,
  `v_zootech` BIT NOT NULL DEFAULT 0 COMMENT 'Отображать в Зоотехплане',
  `v_do_times` SMALLINT UNSIGNED NOT NULL DEFAULT '0' COMMENT 'сколько раз назначать прививку в Зоотехплане(0-всегда)',
  PRIMARY KEY (`v_id`) ,
  UNIQUE INDEX `v_name_UNIQUE` (`v_name` ASC) 
)ENGINE = InnoDB DEFAULT CHARSET = utf8 COMMENT = 'Список имеющихся прививок';

DROP TABLE IF EXISTS `rab_vac`;
CREATE  TABLE `rab_vac` (
  `r_id` INT UNSIGNED NOT NULL ,
  `v_id` TINYINT NOT NULL COMMENT 'Тип прививки' ,
  `date` DATE NULL COMMENT 'Когда была сделана прививка',
  `unabled` BIT NOT NULL DEFAULT 0 COMMENT 'Отменена ли прививка'
)ENGINE = InnoDB DEFAULT CHARSET = utf8 COMMENT = 'Какие прививки делались кролику';

CREATE TABLE IF NOT EXISTS `clients` (
  `c_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `c_org` varchar(45) NOT NULL COMMENT 'название организации',
  `c_address` varchar(100) NOT NULL,
  PRIMARY KEY (`c_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `import_ascendants` (
  `r_id` int(10) unsigned NOT NULL,
  `r_mother` int(10) unsigned NOT NULL DEFAULT '0',
  `r_father` int(10) unsigned NOT NULL DEFAULT '0',
  `r_sex` enum('male','female','void') NOT NULL,
  `r_bon` VARCHAR(5) NOT NULL DEFAULT '10000',
  `r_name` int(10) unsigned NOT NULL DEFAULT '0',
  `r_surname` int(10) unsigned NOT NULL DEFAULT '0',
  `r_secname` int(10) unsigned NOT NULL DEFAULT '0',
  `r_breed` int(10) unsigned NOT NULL DEFAULT '0',
  `r_born` datetime DEFAULT NULL,
  `r_birthplace` int(11) unsigned DEFAULT NULL COMMENT 'программа откуда экспортирован кролик',
  UNIQUE KEY `u_r_id_birtplace` (`r_id`,`r_birthplace`),
  KEY `r_mother` (`r_mother`),
  KEY `r_father` (`r_father`),
  KEY `r_sex` (`r_sex`),
  KEY `r_name` (`r_name`),
  KEY `r_surname` (`r_surname`),
  KEY `r_secname` (`r_secname`),
  KEY `r_breed` (`r_breed`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

#DATA

INSERT INTO `vaccines`(v_id,v_name,v_do_after,v_duration,v_age,v_zootech,v_do_times) VALUES(-1,'Стимуляция самки',0,3,2,0,0);
INSERT INTO `vaccines`(v_id,v_name,v_do_after,v_duration,v_age,v_zootech,v_do_times) VALUES(1,'Прививка',0,180,45,1,0);

INSERT INTO options(o_name,o_subname,o_value) VALUES('db','version','16');
INSERT INTO options(o_name,o_subname,o_value) VALUES
('opt','okrol', 30),
('opt','vudvor', 30),
('opt','vudvor_if_sukrol', 0),
('opt','vudvor_if_proholost', 0),
('opt','vudvor_heater', 0),
('opt','vd_sukr', 0),
('opt','count1', 3),
('opt','count2', 6),
('opt','count3', 13),
('opt','bride', 121),
('opt','candidate', 120),
('opt','preokrol', 27),
('opt','combage', 3),
('opt','malewait', 2),
('opt','girlsout', 100),
('opt','suckers', 50),
('opt','boysout', 80),
('opt','bbone', 90),
('opt','statefuck', 80),
('opt','firstfuck', 60),
('opt','gentree', 1),
('opt','confirmexit', 0),
('opt','confirmkill', 1),
('opt','confirmreplace', 0),
('opt','vacc', 45),
('opt','nest', 16),
('opt','childnest', 21),
('opt','updatezoo', 1),
('opt','findpartner', 1),
('opt','nextsvid', 1),
('opt','svidhead', ''),
('opt','gendir', ''),
('opt','short_names',0),
('opt','dbl_surname',1),
('opt','heterosis',0),
('opt','inbreeding',0),
('opt','sh_tier_t',1),
('opt','sh_tier_s',0),
('opt','sh_num',0),
('opt','short_zoo',1),
('opt','build_fill_zero',0),
('opt','butcher_type',0),
('opt','rab_gen_depth','7');

UPDATE tiers SET t_busy2=NULL,t_busy3=NULL,t_busy4=NULL WHERE t_type='female';
UPDATE tiers SET t_busy3=NULL,t_busy4=NULL WHERE t_type='dfemale' OR t_type='jurta' OR t_type='vertep' OR t_type='barin' OR t_type='cabin';
UPDATE tiers SET t_busy4=NULL WHERE t_type='complex';

INSERT INTO deadreasons(d_name) VALUES
('Списан из старой программы'),
('Объединение'),
('На убой'),#3
('Продажа племенного поголовья'),#4
('Падеж при подсчете'),#5
('Падеж');#6

INSERT INTO logtypes(l_type,l_name,l_params) VALUES
(1,'привоз','$r в $p'),
(2,'пересадка','$r  ->  $A'),
(3,'бонитировка','$r - $t'),
(4,'переименование','$r (было $t)'),
(5,'вязка','$r   + $R ($t)'),
(6,'окрол','$r $t'),
(7,'прохолостание','$r'),
(8,'изменен паспорт кролика','$r'),
(9,'гнездовье установлено','$a ($r)'),
(10,'гнездовье убрано',''),
(11,'грелка убрана','$a ($r)'),
(12,'грелка выключена','$a ($r)'),
(13,'грелка включена','$a ($r)'),
(14,'начат ремонт минифермы','$a'),
(15,'ремонт минифермы окончен','$a'),
(16,'кролик списан','$t (из $a)'),
(17,'подсчет гнездовых','$r $t'),
(18,'изменили пол','$r $t'),
(19,'разбили группу','$r $t'),
(20,'восстановление списанного','$r ($p)'),
(21,'предокрольный осмотр','$r ($p)'),
(22,'объединение групп','$r -> $t($p)'),
(23,'подсадка','$r к $R($P)'),
(24,'Изменение причины списания','$r'),
(25,'Прививка','$r ($t)'),
(26,'изменение Даты рождения','$r ($t)'),
(27,'изменение Породы','$r ($t)'), 
(28,'изменение Количества','$r ($t)'),
(29,'изменение Фамилии по Матери','$r ($t)'),
(30,'изменение Фамилии по Отцу','$r ($t)'),
(31,'изменение Зоны прибытия','$r ($t)'),
(32,'изменение Рейтинга','$r ($t)'),
(33,'изменение Статуса','$r ($t)'),
(34,'Импорт кролика','$r ->$a ($t)'),
(35,'Импорт породы','$a ($t)'),
(36,'Импорт имени','$a ($t)');

INSERT INTO breeds VALUES 
(1,'Гибрид','---','Red'),
(2,'Бабочка','ББ','LightPink'),
(3,'Серебристый','СрБ','Lavender'),
(4,'Черно-Бурый','ЧрБ','Black'),
(5,'Советская Шиншила','СоШ','Turquoise'),
(6,'Венский Голубой','ВеГ','Blue'),
(7,'Белый Великан','БеВ','Gainsboro'),
(8,'Калифорниискии Белыи','КфБ','DarkGray'),
(9,'Серыи Великан','СрВ','DimGray'),
(10,'Гибрид  Ф-1','ГФ-1','White'),
(11,'Гибрид Срб БеВ.','ГФ-2','White'),
(12,'Гибрид СрВ Срб.','ГФ-3','White'),
(13,'Гибрид Кфб БеВ.','ГФ-6','White'),
(14,'Гибрид БеВ КфБ.','ГФ-5','White'),
(15,'Гибрид СрВ БеВ.','ГФ-4','White'),
(16,'Белка','Бл','White'),
(17,'Серо-серебристый','ССрб','DarkRed'),
(18,'Гибрид Срб СрВ','ГФ-7','White'),
(19,'Акселерат','АкС','White'),
(20,'Белка-Сиам','Б-С','White'),
(21,'Новозеландский Белый','НзБ','White'),
(22,'Черепаховый','ЧрП','DarkOrange'),
(23,'Бельгийский Фландр','БеФ','SaddleBrown'),
(24,'Г Бельгийский Фландр','гБеФ','White'),
(25,'Белая Чернова','БеЧ','White');

INSERT INTO `zones` VALUES 
(0,'Ваша ферма','ваша'),
(24000,'Красноярск','Крск'),
(33000,'Владимир','Влад'),
(33001,'Муром','Мур'),
(37000,'Иваново','Иван'),
(37001,'Родники','Род'),
(47000,'С-Петербург','СП'),
(50000,'Москва.','Мск'),
(50001,'Подольск','Под'),
(50002,'Ступино','Стп'),
(50003,'Стулово','СтЛ'),
(50004,'Наро-Фоминск','НарФ'),
(50005,'Озёрки','Озр'),
(54000,'Новосибирск','Нск'),
(62001,'Касимов','Ксм'),
(73000,'Ульяновск','Улн');

INSERT INTO `names` VALUES 
(1,'male','Бич','Бичев',0,NULL),
(2,'male','Баклан','Бакланов',0,NULL),
(3,'male','Боец','Боецов',0,NULL),
(4,'male','Болван','Болванов',0,NULL),
(5,'male','Будда','Буддин',0,NULL),
(6,'male','Самсон','Самсонов',0,NULL),
(7,'male','Сильвер','Сильверов',0,NULL),
(8,'male','Спиноза','Спинозин',0,NULL),
(9,'male','Соломон','Соломонов',0,NULL),
(10,'male','Сударь','Сударев',0,NULL),
(11,'male','Сава','Савин',0,NULL),
(12,'male','Байкал','Байкалов',0,NULL),
(13,'male','Борис','Борисов',0,NULL),
(14,'male','Федор','Федоров',0,NULL),
(15,'male','Бледун','Бледунов',0,NULL),
(16,'female','Сударыня','Сударынин',0,NULL),
(17,'female','Барыня','Барынин',0,NULL),
(18,'female','Бусинка','Бусинкин',0,NULL),
(19,'female','Синьора','Синьорин',0,NULL),
(20,'female','Бурда','Бурдин',0,NULL),
(21,'female','Соня','Сонин',0,NULL),
(22,'female','Бася','Басин',0,NULL),
(23,'female','Бука','Букин',0,NULL),
(24,'female','Бэла','Бэлин',0,NULL),
(25,'female','Сильва','Сильвин',0,NULL),
(26,'female','Сакура','Сакурин',0,NULL),
(27,'female','Сноха','Снохин',0,NULL),
(28,'female','Сумка','Сумкин',0,NULL),
(29,'female','Стерва','Стервин',0,NULL),
(30,'female','Беби','Бебин',0,NULL),
(31,'female','Бирма','Бирмин',0,NULL),
(32,'female','Снежка','Снежкин',0,NULL),
(33,'female','Светка','Светкин',0,NULL),
(34,'female','Блошка','Блошкин',0,NULL),
(35,'female','Сабрина','Сабринин',0,NULL),
(36,'female','Стрелка','Стрелкин',0,NULL),
(37,'female','Шаня','Шанин',0,NULL);

#views

DROP VIEW IF EXISTS allrabbits;
CREATE VIEW allrabbits AS
  (SELECT r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,
 NULL,0,'' FROM rabbits)
UNION
  (SELECT r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,
 d_date,d_reason,d_notes FROM dead);


#DELIMITER |
DROP FUNCTION IF EXISTS rabname |
CREATE FUNCTION rabname (rid INTEGER UNSIGNED ,sur INTEGER) RETURNS CHAR(150)
BEGIN
  DECLARE n,sr,sc,sx,ok CHAR(50);
  DECLARE res CHAR(150);
  DECLARE c INT;
  IF(rid=0 OR rid IS NULL) THEN
    return '';
  END IF;
  SELECT
  (SELECT n_name FROM names WHERE n_id=r_name) name,
  (SELECT n_surname FROM names WHERE n_id=r_surname) surname,
  (SELECT n_surname FROM names WHERE n_id=r_secname) secname,
  r_group,r_sex,r_okrol INTO n,sr,sc,c,sx,ok FROM rabbits WHERE r_id=rid;
  SET res='';
  IF (n IS NOT NULL) THEN
	SET res=n;
  END IF;
  IF (sur>0 AND NOT sr IS NULL) THEN
	IF (res='') THEN
		SET res=sr;
	ELSE
		SET res=CONCAT_WS(' ',res,sr);
	END IF;
    if (c>1) then
      SET res=CONCAT(res,'ы');
    else
      if (sx='female') then SET res=CONCAT(res,'а'); end if;
    end if;
  END IF;
  IF (sur>1 AND NOT sc IS NULL) THEN
    SET res=CONCAT_WS('-',res,sc);
    if (c>1) then
      SET res=CONCAT(res,'ы');
    else
      if (sx='female') then SET res=CONCAT(res,'а'); end if;
    end if;
  END IF;
  IF(n IS NULL) THEN
	SET res=CONCAT_WS('-',res,ok);
  END IF;
  RETURN(res);
END |

DROP FUNCTION IF EXISTS deadname |
CREATE FUNCTION deadname (rid INTEGER UNSIGNED ,sur INTEGER) RETURNS CHAR(150)
BEGIN
  DECLARE n,sr,sc,sx,ok CHAR(50);
  DECLARE res CHAR(150);
  DECLARE c INT;
  IF(rid=0 OR rid IS NULL) THEN
    return '';
  END IF;
  SELECT
  (SELECT n_name FROM names WHERE n_id=r_name) name,
  (SELECT n_surname FROM names WHERE n_id=r_surname) surname,
  (SELECT n_surname FROM names WHERE n_id=r_secname) secname,
  r_group,r_sex,r_okrol INTO n,sr,sc,c,sx,ok FROM dead WHERE r_id=rid;
  SET res='';
  IF (n IS NOT NULL) THEN
	SET res=n;
  END IF;
  IF (sur>0 AND NOT sr IS NULL) THEN
	IF (res='') THEN
		SET res=sr;
	ELSE
		SET res=CONCAT_WS(' ',res,sr);
	END IF;
    if (c>1) then
      SET res=CONCAT(res,'ы');
    else
      if (sx='female') then SET res=CONCAT(res,'а'); end if;
    end if;
  END IF;
  IF (sur>1 AND NOT sc IS NULL) THEN
    SET res=CONCAT_WS('-',res,sc);
    if (c>1) then
      SET res=CONCAT(res,'ы');
    else
      if (sx='female') then SET res=CONCAT(res,'а'); end if;
    end if;
  END IF;
  IF(n IS NULL) THEN
	SET res=CONCAT_WS('-',res,ok);
  END IF;
  RETURN(res);
END |


DROP FUNCTION IF EXISTS isdead |
CREATE FUNCTION isdead(rid INTEGER UNSIGNED) RETURNS BOOL
BEGIN
	DECLARE cnt INTEGER UNSIGNED;
	SELECT COUNT(r_id) INTO cnt FROM rabbits WHERE r_id=rid;
	IF (cnt=0) THEN
		RETURN(1);
	END IF;
	RETURN(0);
END |

DROP FUNCTION IF EXISTS anyname |
CREATE FUNCTION anyname(rid INTEGER UNSIGNED, sur INTEGER) RETURNS CHAR(150)
BEGIN
	IF (isdead(rid)) THEN
		RETURN(deadname(rid,sur));
	END IF;
	RETURN(rabname(rid,sur));
END |

DROP FUNCTION IF EXISTS rabplace |
CREATE FUNCTION rabplace(rid INTEGER UNSIGNED) RETURNS char(150)
BEGIN
  DECLARE res VARCHAR(150);
  DECLARE i1,i2,i3,tid,s1,s2,s3 VARCHAR(20);
  DECLARE par INTEGER UNSIGNED;
  SELECT r_farm,r_tier_id,r_area,r_tier,r_parent
  INTO i1,i2,i3,tid,par
  FROM rabbits WHERE r_id=rid;
  IF (par<>0) THEN
	SELECT r_farm,r_tier_id,r_area,r_tier
	INTO i1,i2,i3,tid
	FROM rabbits WHERE r_id=par;
  END IF;
  IF (tid=0) THEN
    RETURN('');
  END IF;
  SELECT t_type,t_delims,t_nest INTO s1,s2,s3 FROM tiers WHERE t_id=tid;
  IF (ISNULL(s1)) THEN
    RETURN('');
  END IF;
  SET res=CONCAT_WS(',',i1,i2,i3,s1,s2,s3);
  RETURN(res);
END |

DROP FUNCTION IF EXISTS deadplace |
CREATE FUNCTION deadplace(rid INTEGER UNSIGNED) RETURNS char(150)
BEGIN
  DECLARE res VARCHAR(150);
  DECLARE i1,i2,i3,tid,s1,s2,s3 VARCHAR(20);
  SELECT r_farm,r_tier_id,r_area,r_tier
  INTO i1,i2,i3,tid
  FROM dead WHERE r_id=rid;
  IF (tid=0 AND i1=0) THEN
    RETURN('');
  END IF;
  IF (tid<>0) THEN
	SELECT t_type,t_delims,t_nest INTO s1,s2,s3 FROM tiers WHERE t_id=tid;
  ELSE
	SELECT t_type,t_delims,t_nest INTO s1,s2,s3 FROM tiers,minifarms WHERE m_id=i1 AND ((t_id=m_upper AND i2<>1) OR (t_id=m_lower AND i2=1));
  END IF;
  IF (ISNULL(s1)) THEN
    RETURN('');
  END IF;
  SET res=CONCAT_WS(',',i1,i2,i3,s1,s2,s3);
  RETURN(res);
END |

DROP PROCEDURE IF EXISTS killRabbitDate |
CREATE PROCEDURE killRabbitDate (rid INTEGER UNSIGNED,reason INTEGER UNSIGNED,notes TEXT,ddate DATETIME)
BEGIN
  INSERT INTO dead(d_date,d_reason,d_notes,r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,r_parent,r_father,r_mother)
SELECT ddate,reason,notes,r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,r_parent,r_father,r_mother
FROM rabbits WHERE r_id=rid;
DELETE FROM rabbits WHERE r_id=rid;
END |


DROP PROCEDURE IF EXISTS killRabbit |
CREATE PROCEDURE killRabbit (rid INTEGER UNSIGNED,reason INTEGER UNSIGNED,notes TEXT)
BEGIN
  CALL killRabbitDate(rid,reason,notes,NOW());
END |


DROP PROCEDURE IF EXISTS resurrectRabbit |
CREATE PROCEDURE resurrectRabbit(rid INTEGER UNSIGNED)
BEGIN
	INSERT INTO rabbits(r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,r_parent,r_father,r_mother)
 SELECT r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies,
 r_parent,r_father,r_mother
 FROM dead WHERE r_id=rid;
 DELETE FROM dead WHERE r_id=rid;
END |

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

DROP FUNCTION IF EXISTS mealCalculate |
CREATE FUNCTION mealCalculate (id INTEGER UNSIGNED) RETURNS FLOAT
BEGIN
	DECLARE days,i,a,d,sell INTEGER;
	DECLARE res,amnt FLOAT;
	DECLARE sd,ed DateTime;
	SELECT m_start_date, m_end_date INTO sd,ed FROM meal WHERE m_id=id;
	IF(isnull(sd) OR isnull(ed)) THEN
		return 0;
	END IF;
	SELECT to_days(m_end_date)-to_days(m_start_date) INTO days FROM meal WHERE m_id=id;
	IF(days = 0) THEN
		return 0;
	END IF;
	SET i=1; #в день завоза новый корм не расходуется
	SET res=0;
	
	DROP TEMPORARY TABLE IF EXISTS deads;	
	CREATE TEMPORARY TABLE deads AS
		SELECT r_group,r_born,d_date FROM dead    WHERE (AddDate(r_born,18)<=sd OR AddDate(r_born,18)<=ed ) AND d_date>=sd ORDER by r_born;
	
	DROP TEMPORARY TABLE IF EXISTS alives;
	CREATE TEMPORARY TABLE alives AS
		SELECT r_group,r_born  	     FROM rabbits WHERE (AddDate(r_born,18)<=sd OR AddDate(r_born,18)<=ed ) 	 	    ORDER by r_born;
		
	SELECT (select count(*) from deads), (select count(*) from alives) INTO a,d;

	IF a=0 AND d=0 THEN
		return 0;
	END IF;	
		
	WHILE i<=days DO
		SELECT COALESCE(SUM(r_group),0) INTO d FROM deads  WHERE (to_days(AddDate(sd,i))-to_days(r_born))>=18 AND d_date>=AddDate(sd,i);
		SELECT COALESCE(SUM(r_group),0) INTO a FROM alives WHERE (to_days(AddDate(sd,i))-to_days(r_born))>=18;
		SET res=res+d+a;
		SET i=i+1;
	END WHILE;
	SELECT m_amount INTO amnt FROM meal WHERE m_id=id;
	SELECT COALESCE(sum(m_amount),0) INTO sell FROM meal WHERE m_type='out' AND m_start_date BETWEEN sd AND ed;
	SET amnt=amnt-sell;
	IF (amnt<=0) THEN
		return 0;
	END IF;
	IF res=0 THEN
		return 0;
	END IF;
	return (amnt/res);
END|

DROP PROCEDURE IF EXISTS updateMeal |
CREATE PROCEDURE updateMeal()
root:BEGIN
  DECLARE i,oldI,maxId,sell INTEGER;
  DECLARE yngRate FLOAT;
  DECLARE oldSD,oldED,yngSD,yngED DateTime;
  SELECT COALESCE(m_id,0) INTO maxId FROM meal WHERE m_type='in' ORDER BY m_start_date DESC LIMIT 1;
  IF (maxId=0) THEN
	LEAVE root;
  END IF;
  SELECT m_id,m_start_date,m_end_date,m_rate INTO i,yngSD,yngED,yngRate FROM meal WHERE m_type='in' ORDER BY m_start_date ASC LIMIT 1;#id of later date
  WHILE (i<>maxId) DO
    SELECT m_id,m_start_date,COALESCE(m_end_date,'9999-12-31') INTO oldI, oldSD,oldED FROM meal WHERE m_type='in' AND m_start_date>yngSD ORDER BY m_start_date ASC LIMIT 1;
	SELECT COALESCE(sum(m_amount),0) INTO sell FROM meal WHERE m_type='out' AND m_start_date BETWEEN yngSD AND oldED;
	IF (isnull(yngED) OR yngED<>oldSD OR sell<>0 OR isnull(yngRate)) THEN		
		UPDATE meal SET m_end_date=oldSD WHERE m_id=i;
		UPDATE meal SET m_rate=mealCalculate(i) WHERE m_id=i;
	END IF;	
    SET i=oldI;
    SET yngSD=oldSD;
	SET yngED=oldED;
  END WHILE;
END |

## new inbriding

drop procedure if exists `FindRabGenoms`|
CREATE PROCEDURE `FindRabGenoms`(IN rid INT UNSIGNED,INOUT depth SMALLINT UNSIGNED,OUT genoms TEXT)
    READS SQL DATA
root:BEGIN
	DECLARE cnt,mId,fId,mDpt,fDpt INT UNSIGNED DEFAULT 0;
	DECLARE mGens,fGens,pGens TEXT DEFAULT '';
	
	if @@session.max_sp_recursion_depth=0 THEN SET max_sp_recursion_depth=7; END IF;		
	if @@session.max_sp_recursion_depth=depth THEN LEAVE root; END IF;		
	
	SELECT COUNT(*) INTO cnt FROM rabbits WHERE r_id=rid;
	IF cnt!=0 THEN
		SELECT r_mother,r_father INTO mId,fId FROM rabbits WHERE r_id=rid;
	ELSE
		SELECT r_mother,r_father INTO mId,fId FROM dead WHERE r_id=rid;
	END IF;
	
	SELECT depth+1,depth+1 INTO mDpt,fDpt;
	
	IF mId!=0 THEN CALL FindRabGenoms(mId,mDpt,mGens); END IF;
	IF fId!=0 THEN CALL FindRabGenoms(fId,fDpt,fGens); END IF;
	SET mGens = Coalesce(mGens,'');
	SET fGens = Coalesce(fGens,'');
	SELECT IF(mDpt<=fDpt,mDpt,fDpt) INTO depth;
	
	IF mGens !='' AND fGens!='' THEN        
		SET pGens = Concat('{',mGens,',',fGens,'}');
	ELSEIF mGens !='' THEN
		SET pGens = Concat('{',mGens,'}');
	ELSEIF fGens !='' THEN
		SET pGens = Concat('{',fGens,'}');
	END IF;
	
	IF pGens!='' THEN      
		SELECT Concat(rid,':',pGens) INTO genoms;
	ELSE 
		SELECT Concat(rid) INTO genoms;
	END IF;
END|

drop FUNCTION if exists `GetRabGenoms`|
CREATE FUNCTION `GetRabGenoms`(rid INT UNSIGNED) RETURNS text CHARSET cp1251
BEGIN
	DECLARE res TEXT;
	DECLARE depth SMALLINT UNSIGNED DEFAULT 0;
	SET max_sp_recursion_depth = (SELECT Cast(o_value as SIGNED ) FROM options WHERE o_name='opt' AND o_subname='rab_gen_depth');
	CALL FindRabGenoms(rid,depth,res);
	RETURN res;
END|

DROP FUNCTION IF EXISTS `ascname`|
CREATE FUNCTION `ascname`(rid INTEGER UNSIGNED,cid INTEGER UNSIGNED,sur INTEGER) RETURNS char(150)
BEGIN
  DECLARE n,sr,sc,sx CHAR(50);
  DECLARE res CHAR(150);
  IF(rid=0 OR rid IS NULL) THEN
    return '';
  END IF;
  SELECT
	(SELECT n_name FROM names WHERE n_id=r_name) name,
	(SELECT n_surname FROM names WHERE n_id=r_surname) surname,
	(SELECT n_surname FROM names WHERE n_id=r_secname) secname,
	r_sex INTO n,sr,sc,sx FROM import_ascendants WHERE r_id=rid and r_birthplace=cid;
  SET res='';
  IF (n IS NOT NULL) THEN
	SET res=n;
  END IF;
  IF (sur>0 AND NOT sr IS NULL) THEN
	IF (res='') THEN
		SET res=sr;
	ELSE
		SET res=CONCAT_WS(' ',res,sr);
	END IF;
    if (sx='female') then SET res=CONCAT(res,'а'); end if;
  END IF;
  IF (sur>1 AND NOT sc IS NULL) THEN
    SET res=CONCAT_WS('-',res,sc);
    if (sx='female') then SET res=CONCAT(res,'а'); end if;
  END IF;
  RETURN(res);
END |

#DELIMITER ;
