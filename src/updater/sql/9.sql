UPDATE options SET o_value='9' WHERE o_name='db' AND o_subname='version';

DROP TABLE IF EXISTS income;
CREATE TABLE `income` (
  `t_rab_id` INTEGER UNSIGNED NOT NULL,
  `t_date` DATETIME NOT NULL,
  `t_count` INTEGER UNSIGNED,
  PRIMARY KEY (`t_rab_id`)
)
ENGINE = InnoDB COMMENT = 'Таблица привозов';