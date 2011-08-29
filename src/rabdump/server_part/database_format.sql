CREATE DATABASE `rabdump`;

GRANT ALL ON rabdump.* TO `rabdump` IDENTIFIED BY 'rabdump';

CREATE TABLE `users` (
  `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `pass` VARCHAR(50) NOT NULL,
  `farm` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB;

CREATE TABLE `globalReport` (
  `date` DATETIME NOT NULL,
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
ENGINE = InnoDB;

CREATE TABLE `dumplist` (
  `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `datetime` DATETIME NOT NULL,
  `farm` VARCHAR(40) NOT NULL,
  `filename` VARCHAR(100) NOT NULL,
  `md5dump` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`)
)
ENGINE = InnoDB
COMMENT = 'таблица с названиями хнанимых файлов';
