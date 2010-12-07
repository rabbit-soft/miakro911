create temporary table aaa as select r_id,to_days(now())-to_days(r_born) age,r_mother,
(select to_days(now())-to_days(r_born) from allrabbits where r_id=r.r_mother limit 1) m_age,r_father,
(select to_days(now())-to_days(r_born) from allrabbits where r_id=r.r_father limit 1) f_age
from rabbits r where r_id<r_mother or r_id<r_father order by r_id;
create temporary table mmm as select * from aaa where age>m_age;
create temporary table fff as select * from aaa where age>f_age;
update rabbits set r_mother=0 where r_id in (select r_id from mmm);
update rabbits set r_father=0 where r_id in (select r_id from fff);