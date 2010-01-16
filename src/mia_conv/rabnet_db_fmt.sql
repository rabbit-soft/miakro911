DROP TABLE IF EXISTS users;
CREATE TABLE users(
u_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
u_name VARCHAR(50),
u_password VARCHAR(50),
u_group enum('admin','zootech','genetik','worker') NOT NULL DEFAULT 'admin',
KEY(u_name)
);

DROP TABLE IF EXISTS options;
CREATE TABLE options(
	o_name VARCHAR(30) NOT NULL default '',
	o_subname VARCHAR(30) NOT NULL default '',
	o_uid INTEGER UNSIGNED NOT NULL default 0,
	o_value TEXT NOT NULL,
	KEY(o_name,o_subname),
	KEY(o_uid)
);

DROP TABLE IF EXISTS breeds;
CREATE TABLE breeds(
	b_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	b_name VARCHAR(50) NOT NULL,
	b_short_name VARCHAR(4) NOT NULL
);

DROP TABLE IF EXISTS names;
CREATE TABLE names(
	n_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	n_sex ENUM('male','female') NOT NULL, 
	n_name VARCHAR(50) NOT NULL,
	n_surname VARCHAR(50) NOT NULL,
	n_use INTEGER UNSIGNED NOT NULL default 0,
	n_block_date DATETIME DEFAULT NULL,
	KEY(n_sex),
	UNIQUE(n_name),
	KEY(n_use),
	KEY(n_block_date)
);

DROP TABLE IF EXISTS tiers;
CREATE TABLE tiers(
	t_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	t_type ENUM('none','female','dfemale','complex','jurta','quarta','vertep','barin','cabin') NOT NULL,
	t_repair BOOL NOT NULL default 0,
	t_notes TEXT NOT NULL,
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
);

DROP TABLE IF EXISTS minifarms;
CREATE TABLE minifarms(
	m_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	m_upper INTEGER UNSIGNED NULL DEFAULT NULL,
	m_lower INTEGER UNSIGNED NULL DEFAULT NULL,
	KEY(m_upper),
	KEY(m_lower)
);

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
);

DROP TABLE IF EXISTS zones;
CREATE TABLE zones(
	z_id INTEGER UNSIGNED NOT NULL PRIMARY KEY,
	z_name VARCHAR(50) NOT NULL,
	z_short_name VARCHAR(4) NOT NULL
);

DROP TABLE IF EXISTS catalogs;
CREATE TABLE catalogs(
 c_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
 c_type ENUM('partner','kind','name') NOT NULL,
 c_name VARCHAR(50) NOT NULL default '',
 c_value TEXT NOT NULL,
 c_flags VARCHAR(30) NOT NULL default '',
 KEY(c_type),
 KEY(c_name),
 key(c_flags)
);


DROP TABLE IF EXISTS transfers;
CREATE TABLE transfers(
t_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
t_type ENUM('rabbits','meat','skin','feed','feed_use','otsev','other') NOT NULL,
t_notes TEXT NOT NULL,
t_date DATETIME NOT NULL,
t_units INTEGER UNSIGNED NOT NULL DEFAULT 1,
t_sold BOOL NOT NULL DEFAULT 0,
t_age INTEGER UNSIGNED,
t_weight INTEGER UNSIGNED,
t_weight2 INTEGER UNSIGNED,
t_price DOUBLE,
t_partner INTEGER UNSIGNED,
t_mdate DATETIME,
t_sex TINYINT UNSIGNED,
t_breed INTEGER UNSIGNED,
t_kind INTEGER UNSIGNED,
t_name INTEGER UNSIGNED,
t_rabbit INTEGER UNSIGNED,
t_str TEXT,
KEY(t_type),
KEY(t_date),
KEY(t_sold)
);

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
	r_notes TEXT NOT NULL,
	r_okrol INTEGER UNSIGNED NOT NULL default 0,
	r_farm INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_tier INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_tier_id TINYINT UNSIGNED NOT NULL DEFAULT 0,
	r_area INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_rate INTEGER NOT NULL default 0,
	r_group INTEGER UNSIGNED NOT NULL DEFAULT 1,
	r_breed INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_flags VARCHAR(10) NOT NULL DEFAULT '000000', #butcher | risk | multi(brak|vakcin) | nokuk | nolack
	r_zone INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_born DATETIME,
	r_genesis INTEGER UNSIGNED NOT NULL,
	r_status TINYINT UNSIGNED NOT NULL DEFAULT 0,   #boy-status/girl-borns
	r_last_fuck_okrol DATETIME,
#	r_children TINYINT UNSIGNED,
	r_event ENUM('none','sluchka','vyazka','kuk'),
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
	
);

DROP TABLE IF EXISTS fucks;
CREATE TABLE fucks(
	f_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	f_rabid INTEGER UNSIGNED NOT NULL,
	f_date DATETIME DEFAULT NULL,
	f_type enum('sluchka','vyazka','kuk') NOT NULL default 'vyazka',
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
	f_notes TEXT NOT NULL ,
	KEY(f_date),
	KEY(f_state),
	KEY(f_type),
	KEY(f_children),
	KEY(f_dead)
);

DROP TABLE IF EXISTS workers;
CREATE TABLE workers(
	w_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	w_name VARCHAR(50) NOT NULL,
	w_spec VARCHAR(30) NOT NULL default '',
	w_rate INTEGER NOT NULL default 0,
	w_notes TEXT
);

DROP TABLE IF EXISTS genesis;
CREATE TABLE genesis(
	g_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	g_notes TEXT NOT NULL,
	g_key VARCHAR(50),
	KEY(g_id),
	KEY(g_key)
);

DROP TABLE IF EXISTS genoms;
CREATE TABLE genoms(
	g_id INTEGER UNSIGNED NOT NULL,
	g_genom INTEGER UNSIGNED NOT NULL,
	KEY(g_id),
	KEY(g_genom),
	UNIQUE(g_id,g_genom)
);

DROP TABLE IF EXISTS weights;
CREATE TABLE weights(
	w_rabid INTEGER UNSIGNED NOT NULL,
	w_date DATETIME NOT NULL,
	w_weight INTEGER UNSIGNED NOT NULL,
	KEY(w_rabid)
);

DROP TABLE IF EXISTS jobs;
CREATE TABLE jobs(
	j_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	j_name VARCHAR(50) NOT NULL,
	j_short_name VARCHAR(4) NOT NULL
);

DROP TABLE IF EXISTS zooplans;
CREATE TABLE zooplans(
	z_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	z_done INTEGER NOT NULL default 0,
	z_date DATETIME NOT NULL,
	z_job INTEGER UNSIGNED NOT NULL default 0,
	z_level INTEGER UNSIGNED NOT NULL default 0,
	z_rabbit INTEGER UNSIGNED,
	z_rabbit2 INTEGER UNSIGNED,
	z_address INTEGER UNSIGNED,
	z_address2 INTEGER UNSIGNED,
	z_flags VARCHAR(50),
	z_notes TEXT,
	z_memo TEXT,
	z_user INTEGER UNSIGNED NOT NULL DEFAULT 0,
	KEY(z_done),
	KEY(z_date),
	KEY(z_job),
	KEY(z_level),
	KEY(z_rabbit),
	KEY(z_address),
	KEY(z_user)
);

DROP TABLE IF EXISTS zooacceptors;
CREATE TABLE zooacceptors(
	z_id INTEGER UNSIGNED NOT NULL,
	z_rabbit INTEGER UNSIGNED,
	z_lack INTEGER,
	z_hybrid BOOL,
	z_new_group BOOL,
	z_gendiff INTEGER,
	z_distance INTEGER,
	z_best_donor INTEGER UNSIGNED,
	z_best_acceptor INTEGER UNSIGNED,
	KEY(z_id)
);


DROP TABLE IF EXISTS archive;
CREATE TABLE archive(
	a_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	a_date DATETIME NOT NULL,
	a_level INTEGER,
	a_job VARCHAR(50),
	a_address VARCHAR(100),
	a_name VARCHAR(50),
	a_age INTEGER UNSIGNED,
	a_partners VARCHAR(100),
	a_addresses VARCHAR(100),
	a_notes TEXT,
	KEY(a_date)
);

DROP TABLE IF EXISTS deadreasons;
CREATE TABLE deadreasons(
	d_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	d_name VARCHAR(50) NOT NULL,
	d_rate INTEGER NOT NULL default 0
);

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
	r_notes TEXT NOT NULL,
	r_okrol INTEGER UNSIGNED NOT NULL default 0,
	r_farm INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_tier INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_tier_id TINYINT UNSIGNED NOT NULL DEFAULT 0,
	r_area INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_rate INTEGER NOT NULL default 0,
	r_group INTEGER UNSIGNED NOT NULL DEFAULT 1,
	r_breed INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_flags VARCHAR(10) NOT NULL DEFAULT '000000',
	r_zone INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_born DATETIME,
	r_genesis INTEGER UNSIGNED NOT NULL,
	r_status TINYINT UNSIGNED NOT NULL DEFAULT 0,
	r_last_fuck_okrol DATETIME,
	r_lost_babies INTEGER UNSIGNED,
	r_overall_babies INTEGER UNSIGNED,
	KEY(d_date),
	KEY(d_reason),
	KEY(r_id)
);

DROP TABLE IF EXISTS filters;
CREATE TABLE filters(
	f_type VARCHAR(30) NOT NULL,
	f_name VARCHAR(30) NOT NULL,
	f_filter TEXT,
	KEY(f_type),
	KEY(f_name)
);

DROP TABLE IF EXISTS logtypes;
CREATE TABLE logtypes(
	l_type INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	l_name VARCHAR(30) NOT NULL,
	l_params TEXT NOT NULL,
	KEY(l_name)
);

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
);

#DATA

UPDATE tiers SET t_busy2=NULL,t_busy3=NULL,t_busy4=NULL WHERE t_type='female';
UPDATE tiers SET t_busy3=NULL,t_busy4=NULL WHERE t_type='dfemale' OR t_type='jurta' OR t_type='vertep' OR t_type='barin' OR t_type='cabin';
UPDATE tiers SET t_busy4=NULL WHERE t_type='complex';
INSERT INTO logtypes(l_name,l_params) VALUES
('привоз','$r в $p'),
('пересадка','$r из $a в $p'),
('бонитировка','$r - $t'),
('переименование','$r (было $t)'),
('вязка','$r c $R'),
('окрол','$r $t'),
('прохолостание','$r'),
('изменен паспорт кролика','$r'),
('гнездовье установлено','$a ($r)'),
('гнездовье убрано','$a ($r)'),
('грелка убрана','$a ($r)'),
('грелка выключена','$a ($r)'),
('грелка включена','$a ($r)'),
('начат ремонт минифермы','$a'),
('ремонт минифермы окончен','$a'),
('кролик списан','$t (из $a)'),
('подсчет гнездовых','$r $t'),
('изменили пол','$r $t'),
('разбили группу','$r $t')
;


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

DROP FUNCTION IF EXISTS rabplace |
CREATE FUNCTION rabplace(rid INTEGER UNSIGNED) RETURNS char(150)
BEGIN
  DECLARE res VARCHAR(150);
  DECLARE i1,i2,i3,s1,s2,s3 VARCHAR(20);
  SELECT r_farm,r_tier_id,r_area,t_type,t_delims,t_nest
  INTO i1,i2,i3,s1,s2,s3
  FROM rabbits,tiers WHERE r_id=rid AND t_id=r_tier;
  SET res=CONCAT_WS(',',i1,i2,i3,s1,s2,s3);
  RETURN(res);
END |

DROP PROCEDURE IF EXISTS killRabbitDate |
CREATE PROCEDURE killRabbitDate (rid INTEGER UNSIGNED,reason INTEGER UNSIGNED,notes TEXT,ddate DATETIME)
BEGIN
  INSERT INTO dead(d_date,d_reason,d_notes,r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies)
SELECT ddate,reason,notes,r_id,r_sex,r_bon,r_name,r_surname,r_secname,
 r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
 r_born,r_genesis,r_status,r_last_fuck_okrol,r_lost_babies,r_overall_babies
FROM rabbits WHERE r_id=rid;
DELETE FROM rabbits WHERE r_id=rid;
END |


DROP PROCEDURE IF EXISTS killRabbit |
CREATE PROCEDURE killRabbit (rid INTEGER UNSIGNED,reason INTEGER UNSIGNED,notes TEXT)
BEGIN
  CALL killRabbitDate(rid,reason,notes,NOW());
END |
#DELIMITER ;

##TEST_DATA do not remove this line

INSERT INTO users(u_name,u_password) VALUES('john',MD5(''));
