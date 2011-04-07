UPDATE options SET o_value='7' WHERE o_name='db' AND o_subname='version';

CREATE TABLE `kroliki`.`products` (
  `p_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `p_name` VARCHAR(45) NOT NULL DEFAULT '' COMMENT 'название продукции',
  `p_unit` VARCHAR(30) NOT NULL DEFAULT '' 'единица измерения',
  `p_image` BLOB COMMENT 'изображение',
  `p_imgsize` INTEGER UNSIGNED  COMMENT 'размер изображения',
  PRIMARY KEY (`p_id`)
)ENGINE = InnoDB COMMENT = 'продукция получаемая из кролика';

ALTER TABLE `kroliki`.`users` MODIFY COLUMN `u_group` ENUM('worker','admin','zootech','butcher') NOT NULL DEFAULT 'admin';

CREATE TABLE `kroliki`.`butcher` (
  `b_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `b_date` DATETIME NOT NULL COMMENT 'дата взвешивания',
  `b_prodtype` INTEGER UNSIGNED NOT NULL COMMENT 'тип продукта',
  `b_amount` FLOAT UNSIGNED NOT NULL COMMENT 'количество ГП',
  `b_user` INTEGER UNSIGNED NOT NULL COMMENT 'пользователь',
  PRIMARY KEY (`b_id`)
)
ENGINE = InnoDB
COMMENT = 'готовая продукция';

INSERT INTo logtypes (l_name,l_params) VALUES("Изменение причины списания","$r");