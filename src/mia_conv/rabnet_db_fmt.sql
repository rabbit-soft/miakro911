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
	n_sex BOOL NOT NULL DEFAULT 0, 
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
	t_type ENUM('female','dfemale','complex','jurta','quarta','vertep','barin','cabin') NOT NULL,
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
	m_tier_upper INTEGER UNSIGNED NULL DEFAULT NULL,
	m_tier_lower INTEGER UNSIGNED NULL DEFAULT NULL,
	m_name VARCHAR(50) NULL,
	m_parent INTEGER UNSIGNED NOT NULL default 0,
	m_level INTEGER UNSIGNED NOT NULL default 0,
	KEY(m_tier_upper),
	KEY(m_tier_lower),
	KEY(m_name),
	KEY(m_parent)
);

DROP TABLE IF EXISTS zones;
CREATE TABLE zones(
	z_id INTEGER UNSIGNED NOT NULL PRIMARY KEY,
	z_name VARCHAR(50) NOT NULL,
	z_short_name VARCHAR(4) NOT NULL
);



##TEST_DATA do not remove this line

INSERT INTO users(u_name,u_password) VALUES('john',MD5(''));
