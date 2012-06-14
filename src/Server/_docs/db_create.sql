CREATE DATABASE `rabserv`;

GRANT ALL ON rabserv.* TO `rabserv` IDENTIFIED BY 'rabserv';

DROP TABLE IF EXISTS `rabserv`.`clients`;
CREATE TABLE  `rabserv`.`clients` (
  `c_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `c_org` varchar(45) NOT NULL COMMENT 'name of the organization',
  `c_address` varchar(100) NOT NULL,
  `c_contact` varchar(45) NOT NULL COMMENT 'name of contact man',
  `c_money` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'money on the account',
  `c_saas` tinyint(1) NOT NULL DEFAULT '0',
  `c_login` varchar(45) DEFAULT NULL,
  `c_pwd` varchar(45) DEFAULT NULL,
  `c_key` blob,
  `c_cost_saas` tinyint(4) NOT NULL DEFAULT '5',
  `c_cost_box` tinyint(4) NOT NULL DEFAULT '100',
  PRIMARY KEY (`c_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;


CREATE TABLE `rabserv`.`dongles` (
  `d_id` int(10) unsigned NOT NULL,
  `d_client` int(10) unsigned NOT NULL COMMENT 'id клиента',
  `d_model` tinyint(10) unsigned NOT NULL,
  `d_returned` tinyint(1) NOT NULL default '0' COMMENT 'вернули ли ключ'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `rabserv`.`money` (
  `m_id` int(10) unsigned NOT NULL auto_increment,
  `m_client` int(10) unsigned NOT NULL,
  `m_date` datetime NOT NULL,
  `m_debet` int(10) unsigned NOT NULL DEFAULT 0 COMMENT 'income money',
  `m_credit` int(10) unsigned NOT NULL DEFAULT 0 COMMENT 'outcome money',
  `m_comment` varchar(45) NOT NULL,
  PRIMARY KEY  (`m_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=49 ;

CREATE TABLE `rabserv`.`options` (
  `o_name` varchar(10) NOT NULL,
  `o_subname` varchar(10) NOT NULL,
  `o_value` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='опции';

CREATE TABLE `rabserv`.`updates` (
  `u_id` int(10) unsigned NOT NULL auto_increment,
  `u_dongle` int(10) unsigned NOT NULL,
  `u_client` int(10) unsigned NOT NULL,
  `u_date` datetime NOT NULL,
  `u_waiting` tinyint(1) NOT NULL default '0' COMMENT 'update_mask for remote dongle, which connects to server',
  `u_farms` varchar(45) NOT NULL COMMENT 'buildings count',
  `u_start_date` datetime NOT NULL,
  `u_end_date` datetime NOT NULL,
  `u_flags` int(10) unsigned NOT NULL,
  `u_time_flags` int(10) unsigned NOT NULL,
  `u_time_flags_end` datetime NOT NULL,
  PRIMARY KEY  (`u_id`),
  KEY `Index_1` (`u_dongle`,`u_client`,`u_date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=54
COMMENT = 'таблица с записями об обновлении ключей';

CREATE TABLE `rabserv`.`users` (
  `u_id` int(10) unsigned NOT NULL auto_increment,
  `u_name` varchar(45) NOT NULL,
  `u_key` blob NOT NULL,
  `u_new_key` blob NOT NULL,
  PRIMARY KEY  (`u_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=2
COMMENT = 'таблица с администраторами';

CREATE TABLE `rabserv`.`globalReport` (
  `date` DATETIME NOT NULL,
  `clientId` int(10) unsigned NOT NULL,
  `farm` VARCHAR(45) NOT NULL,
  `database` VARCHAR(45) NOT NULL COMMENT 'название БД на компьютере пользователя',
  `fucks` INTEGER UNSIGNED NOT NULL DEFAULT 0 COMMENT 'случек',
  `okrols` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `proholosts` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `born` INTEGER UNSIGNED NOT NULL DEFAULT 0 COMMENT 'сколько рождено крольчат',
  `killed` INTEGER UNSIGNED NOT NULL DEFAULT 0 COMMENT 'сколько забито',
  `deads` INTEGER UNSIGNED NOT NULL,
  `rabbits` INTEGER UNSIGNED NOT NULL COMMENT 'количество кроликов на ферме',
  UNIQUE INDEX `Indexes`(`date`, `farm`, `database`)
)
ENGINE = InnoDB
COMMENT = 'таблица с отчетами по фермам';

CREATE TABLE `rabserv`.`dumplist` (
  `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `datetime` DATETIME NOT NULL,
  `clientId` int(10) unsigned NOT NULL,
  `farm` VARCHAR(40) NOT NULL,
  `filename` VARCHAR(100) NOT NULL,
  `md5dump` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
COMMENT = 'таблица с названиями хнанимых файлов';