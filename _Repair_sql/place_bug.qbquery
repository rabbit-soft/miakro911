select r_id,rabname(r_id),rabplace(r_id) from rabbits where r_id not in(select t_busy1 t from tiers where t_busy1<>0 union
select t_busy2 t from tiers where t_busy2<>0 union
select t_busy3 t from tiers where t_busy3<>0 and t_busy3 is not null union
select t_busy4 t from tiers where t_busy4<>0 and t_busy4 is not null);