select t from (select t_busy1 t from tiers where t_busy1<>0 union
	select t_busy2 t from tiers where t_busy2<>0 union
	select t_busy3 t from tiers where t_busy3<>0 and t_busy3 is not null union
	select t_busy4 t from tiers where t_busy4<>0 and t_busy4 is not null) bus 
where t not in(select r_id from rabbits);