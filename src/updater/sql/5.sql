UPDATE options SET o_value='5' WHERE o_name='db' AND o_subname='version';

ALTER TABLE rabbits ENGINE = MyISAM;
INSERT INTO options (o_name,o_subname,o_uid,o_value) VALUES ('opt','candidate',0,120); 									
UPDATE rabbits SET r_last_fuck_okrol = null WHERE r_sex='male' AND r_status=0;
		
CREATE TEMPORARY TABLE aaa SELECT r_id, r_lost_babies, r_overall_babies FROM rabbits WHERE r_sex='female' AND r_overall_babies < r_lost_babies;	
UPDATE rabbits r, aaa SET r.r_overall_babies = aaa.r_lost_babies, r.r_lost_babies=aaa.r_overall_babies WHERE r.r_id = aaa.r_id;

/* удаление r_mother, r_father у кролей кто старше своих родителей

CREATE TEMPORARY TABLE bbb AS SELECT r_id,to_days(NOW())-to_days(r_born) age,
  r_mother,(SELECT to_days(now())-to_days(r_born) FROM allrabbits WHERE r_id=r.r_mother) m_age,
  r_father,(SELECT to_days(now())-to_days(r_born) FROM allrabbits WHERE r_id=r.r_father) f_age
FROM rabbits r WHERE r_id<r_mother OR r_id<r_father;
CREATE TEMPORARY TABLE mmm AS SELECT r_id FROM bbb WHERE age>m_age;
CREATE TEMPORARY TABLE fff AS SELECT r_id FROM bbb WHERE age>f_age;
UPDATE rabbits SET r_mother=0 WHERE r_id in (SELECT r_id from mmm);
UPDATE rabbits SET r_father=0 WHERE r_id in (SELECT r_id from fff);
SELECT r_id,to_days(NOW())-to_days(r_born) age,
  r_mother,(SELECT to_days(now())-to_days(r_born) FROM allrabbits WHERE r_id=r.r_mother) m_age,
  r_father,(SELECT to_days(now())-to_days(r_born) FROM allrabbits WHERE r_id=r.r_father) f_age
FROM rabbits r WHERE r_id<r_mother OR r_id<r_father;

 конец удаления*/
		