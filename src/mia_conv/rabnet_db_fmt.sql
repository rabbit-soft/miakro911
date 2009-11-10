DROP TABLE IF EXISTS users;

CREATE TABLE users(
u_id INTEGER UNSIGNED NOT NULL PRIMARY KEY AUTO_INCREMENT,
u_name VARCHAR(50),
u_password VARCHAR(50),
KEY(u_name)
);



##TEST DATA
INSERT INTO users(u_name,u_password) VALUES('john',MD5(''));