-- 
-- База данных: `grdupdate`
-- 

-- 
-- Структура таблицы `clients`
-- 

CREATE TABLE `grdupdate`;

DROP TABLE IF EXISTS `clients`;
CREATE TABLE  `clients` (
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

-- 
-- Структура таблицы `dongles`
-- 

CREATE TABLE `dongles` (
  `d_id` int(10) unsigned NOT NULL,
  `d_client` int(10) unsigned NOT NULL COMMENT 'id клиента',
  `d_model` tinyint(10) unsigned NOT NULL,
  `d_returned` tinyint(1) NOT NULL default '0' COMMENT 'вернули ли ключ'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Структура таблицы `money`
-- 

CREATE TABLE `money` (
  `m_id` int(10) unsigned NOT NULL auto_increment,
  `m_client` int(10) unsigned NOT NULL,
  `m_date` datetime NOT NULL,
  `m_debet` int(10) unsigned NOT NULL DEFAULT 0 COMMENT 'income money',
  `m_credit` int(10) unsigned NOT NULL DEFAULT 0 COMMENT 'outcome money',
  `m_comment` varchar(45) NOT NULL,
  PRIMARY KEY  (`m_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=49 ;

-- 
-- Структура таблицы `options`
-- 

CREATE TABLE `options` (
  `o_name` varchar(10) NOT NULL,
  `o_subname` varchar(10) NOT NULL,
  `o_value` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='опции';

-- 
-- Структура таблицы `updates`
-- 

CREATE TABLE `updates` (
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
  `u_id` int(10) unsigned NOT NULL auto_increment,
  PRIMARY KEY  (`u_id`),
  KEY `Index_1` (`u_dongle`,`u_client`,`u_date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=54 ;

-- 
-- Структура таблицы `users`
-- 

CREATE TABLE `users` (
  `u_id` int(10) unsigned NOT NULL auto_increment,
  `u_name` varchar(45) NOT NULL,
  `u_key` blob NOT NULL,
  `u_new_key` blob NOT NULL,
  PRIMARY KEY  (`u_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;