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
	o_value TEXT ,
	KEY(o_name,o_subname),
	KEY(o_uid)
);

DROP TABLE IF EXISTS filters;
CREATE TABLE filters(
	f_type VARCHAR(30) NOT NULL,
	f_name VARCHAR(30) NOT NULL,
	f_filter TEXT,
	KEY(f_type),
	KEY(f_name)
);

DROP TABLE IF EXISTS breeds;
CREATE TABLE breeds(
	b_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	b_name VARCHAR(50) NOT NULL,
	b_short_name VARCHAR(10) NOT NULL,
	b_color VARCHAR(100) NOT NULL default "White"
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
	r_flags VARCHAR(10) NOT NULL DEFAULT '00000', #butcher | risk | multi(brak|vakcin) | nokuk | nolack
	r_zone INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_born DATETIME,
	r_genesis INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_status TINYINT UNSIGNED NOT NULL DEFAULT 0,   #boy-status/girl-borns
	r_last_fuck_okrol DATETIME,
	r_event ENUM('none','sluchka','vyazka','kuk'),
	r_event_date DATETIME,
	r_lost_babies INTEGER UNSIGNED,
	r_overall_babies INTEGER UNSIGNED,
	r_vaccine_end DATETIME,
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
	
) ENGINE = MyISAM;

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
	f_notes TEXT,
	KEY(f_date),
	KEY(f_state),
	KEY(f_type),
	KEY(f_children),
	KEY(f_dead)
);

DROP TABLE IF EXISTS genesis;
CREATE TABLE genesis(
	g_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	g_notes TEXT,
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
	r_genesis INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_status TINYINT UNSIGNED NOT NULL DEFAULT 0,
	r_last_fuck_okrol DATETIME,
	r_lost_babies INTEGER UNSIGNED,
	r_overall_babies INTEGER UNSIGNED,
	r_vaccine_end DATETIME,
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
);

DROP TABLE IF EXISTS logtypes;
CREATE TABLE logtypes(
	l_type INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	l_name VARCHAR(30) NOT NULL,
	l_params TEXT,
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

INSERT INTO options(o_name,o_subname,o_value) VALUES('db','version','5');
INSERT INTO options(o_name,o_subname,o_value) VALUES
('opt', 'okrol', 30),
('opt', 'vudvor', 30),
('opt', 'count1', 3),
('opt', 'count2', 6),
('opt', 'count3', 13),
('opt', 'bride', 121),
('opt', 'candidate', 120),
('opt', 'preokrol', 27),
('opt', 'combage', 3),
('opt', 'malewait', 2),
('opt', 'girlsout', 100),
('opt', 'suckers', 50),
('opt', 'boysout', 80),
('opt', 'statefuck', 80),
('opt', 'firstfuck', 60),
('opt', 'gentree', 10),
('opt', 'confirmexit', 0),
('opt', 'confirmkill', 1),
('opt', 'vacc', 45),
('opt', 'nest', 16),
('opt', 'childnest', 21),
('opt', 'updatezoo', 1),
('opt', 'findpartner', 1),
('opt', 'nextsvid', 1),
('opt', 'svidhead', ''),
('opt', 'gendir', ''),
('opt','short_names',0),
('opt','dbl_surname',1),
('opt','heterosis',0),
('opt','inbreeding',0),
('opt','sh_tier_t',1),
('opt','sh_tier_s',0),
('opt','sh_num',0),
('opt','short_zoo',1),
('opt','vaccine_time',0);

UPDATE tiers SET t_busy2=NULL,t_busy3=NULL,t_busy4=NULL WHERE t_type='female';
UPDATE tiers SET t_busy3=NULL,t_busy4=NULL WHERE t_type='dfemale' OR t_type='jurta' OR t_type='vertep' OR t_type='barin' OR t_type='cabin';
UPDATE tiers SET t_busy4=NULL WHERE t_type='complex';
INSERT INTO deadreasons(d_name) VALUES
('Списан из старой программы'),
('объединение'),
('На убой'),
('Продажа племенного поголовья'),
('Естественный отход'),
('Падеж');
INSERT INTO logtypes(l_name,l_params) VALUES
('привоз','$r в $p'),
('пересадка','$r  ->  $A'),
('бонитировка','$r - $t'),
('переименование','$r (было $t)'),
('вязка','$r   + $R'),
('окрол','$r $t'),
('прохолостание','$r'),
('изменен паспорт кролика','$r'),
('гнездовье установлено','$a ($r)'),
('гнездовье убрано',''),
('грелка убрана','$a ($r)'),
('грелка выключена','$a ($r)'),
('грелка включена','$a ($r)'),
('начат ремонт минифермы','$a'),
('ремонт минифермы окончен','$a'),
('кролик списан','$t (из $a)'),
('подсчет гнездовых','$r $t'),
('изменили пол','$r $t'),
('разбили группу','$r $t'),
('восстановление списанного','$r ($p)'),
('предокрольный осмотр','$r ($p)'),
('объединение групп','$r ($p) $t'),
('подсадка','$r к $R($P)')
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
  SET res=CONCAT_WS(',',i1,i2,i3,s1,s2,s3);
  RETURN(res);
END |

DROP PROCEDURE IF EXISTS killRabbitDate |
CREATE PROCEDURE killRabbitDate (rid INTEGER UNSIGNED,reason INTEGER UNSIGNED,notes TEXT,ddate DATETIME)
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

#DELIMITER ;
