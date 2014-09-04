UPDATE options SET o_value = '16' WHERE o_name = 'db' AND o_subname = 'version';

ALTER TABLE  `users` ADD  `u_deleted` TINYINT( 1 ) NOT NULL DEFAULT  '0';