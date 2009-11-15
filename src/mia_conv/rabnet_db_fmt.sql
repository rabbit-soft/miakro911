DROP TABLE IF EXISTS users;
CREATE TABLE users(
u_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
u_name VARCHAR(50),
u_password VARCHAR(50),
KEY(u_name)
);

DROP TABLE IF EXISTS options;
CREATE TABLE options(
	o_name VARCHAR(30) NOT NULL default '',
	o_subname VARCHAR(30) NOT NULL default '',
	o_uid INTEGER UNSIGNED NOT NULL default 0,
	o_value TEXT NOT NULL default '',
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
	KEY(n_name),
	KEY(n_use),
	KEY(n_block_date)
);

DROP TABLE IF EXISTS tiers;
CREATE TABLE tiers(
	t_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	t_type ENUM('none','female','dfemale','complex','jurta','quarta','vertep','barin','cabin') NOT NULL,
	t_repair BOOL NOT NULL default 0,
	t_notes TEXT NOT NULL default '',
	t_busy1 INTEGER UNSIGNED NOT NULL default 0,
	t_busy2 INTEGER UNSIGNED NOT NULL default 0,
	t_busy3 INTEGER UNSIGNED NOT NULL default 0,
	t_busy4 INTEGER UNSIGNED NOT NULL default 0,
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
 c_value TEXT NOT NULL default '',
 c_flags VARCHAR(30) NOT NULL default '',
 KEY(c_type),
 KEY(c_name),
 key(c_flags)
);


DROP TABLE IF EXISTS transfers;
CREATE TABLE transfers(
t_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
t_type ENUM('rabbits','meat','skin','feed','feed_use','otsev','other') NOT NULL,
t_notes TEXT NOT NULL DEFAULT '',
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
	r_sex ENUM('male','female','void') NOT NULL,
	r_bon VARCHAR(5) NOT NULL DEFAULT '10000',
	r_number INTEGER UNSIGNED NOT NULL DEFAULT 0,  #hz number
	r_unique INTEGER UNSIGNED NOT NULL default 0,	#hz number 2
	r_name INTEGER UNSIGNED NOT NULL default 0,
	r_surname INTEGER UNSIGNED NOT NULL default 0,
	r_secname INTEGER UNSIGNED NOT NULL default 0,
	r_notes TEXT NOT NULL DEFAULT '',
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
	r_status TINYINT UNSIGNED NOT NULL DEFAULT 0,   #boy-status/girl-child_counts
	r_last_fuck_okrol DATETIME,
	r_borns TINYINT UNSIGNED,
	r_event ENUM('none','sluchka','vyazka','kuk'),
	r_event_date DATETIME,
	r_lost_babies INTEGER UNSIGNED,
	r_overall_babies INTEGER UNSIGNED,
	r_worker INTEGER UNSIGNED,
	KEY(r_parent),	KEY(r_number),
	KEY(r_sex),
	KEY(r_unique),
	KEY(r_name),KEY(r_surname),KEY(r_secname),
	KEY(r_farm),KEY(r_tier),
	KEY(r_group),
	KEY(r_breed),
	KEY(r_zone),
	KEY(r_status),
	KEY(r_born)
	
);

DROP TABLE IF EXISTS fuckers;
CREATE TABLE fuckers(
	f_rabid INTEGER UNSIGNED NOT NULL,
	f_live BOOL NOT NULL DEFAULT 1,
	f_link INTEGER UNSIGNED NOT NULL DEFAULT 0,
	f_genesis INTEGER UNSIGNED NOT NULL,
	f_breed INTEGER UNSIGNED NOT NULL DEFAULT 0,
	f_fucks INTEGER UNSIGNED NOT NULL DEFAULT 0,
	f_children INTEGER UNSIGNED NOT NULL DEFAULT 0,
	f_lastfuck BOOL NOT NULL DEFAULT 0,
	KEY(f_rabid),
	KEY(f_live),
	KEY(f_link)
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
	g_notes TEXT NOT NULL default '',
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
	z_done BOOL NOT NULL default 0,
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
	KEY(z_done),
	KEY(z_date),
	KEY(z_job),
	KEY(z_level),
	KEY(z_rabbit),
	KEY(z_address)
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

DROP TABLE IF EXISTS dropreasons;
CREATE TABLE dropreasons(
	d_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	d_name VARCHAR(50) NOT NULL,
	d_rate INTEGER NOT NULL default 0
);

DROP TABLE IF EXISTS drops;
CREATE TABLE drops(
	d_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	d_date DATETIME NOT NULL,
	d_name VARCHAR(100) NOT NULL,
	d_address VARCHAR(50) NOT NULL,
	d_sex ENUM('void','male','female') NOT NULL,
	d_state VARCHAR(10) NOT NULL,
	d_age INTEGER UNSIGNED NOT NULL default 0,
	d_weight INTEGER UNSIGNED NOT NULL default 0,
	d_notes TEXT,
	d_reason INTEGER UNSIGNED NOT NULL default 0,
	d_worker INTEGER UNSIGNED NOT NULL default 0,
	KEY(d_date)
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


DROP TABLE IF EXISTS dead;
CREATE TABLE dead(
	d_date DATETIME NOT NULL,
	r_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
	r_parent INTEGER UNSIGNED NOT NULL default 0,
	r_sex ENUM('male','female','void') NOT NULL,
	r_bon VARCHAR(5) NOT NULL DEFAULT '10000',
	r_number INTEGER UNSIGNED NOT NULL DEFAULT 0,
	r_unique INTEGER UNSIGNED NOT NULL default 0,
	r_name INTEGER UNSIGNED NOT NULL default 0,
	r_surname INTEGER UNSIGNED NOT NULL default 0,
	r_secname INTEGER UNSIGNED NOT NULL default 0,
	r_notes TEXT NOT NULL DEFAULT '',
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
	r_borns TINYINT UNSIGNED,
	r_event ENUM('none','sluchka','vyazka','kuk'),
	r_event_date DATETIME,
	r_lost_babies INTEGER UNSIGNED,
	r_overall_babies INTEGER UNSIGNED,
	r_worker INTEGER UNSIGNED,
	KEY(d_date)
);


##TEST_DATA do not remove this line

INSERT INTO users(u_name,u_password) VALUES('john',MD5(''));
