## -- иногда происходит, что кролика списали, но он все еще сидит в клетке. От этого клетка нигде не фигурирует
select t_id, rid, p
from (
	select t_id, t_busy1 rid, 1 AS p from tiers where t_busy1<>0 union
	select t_id, t_busy2 rid, 2 AS p from tiers where t_busy2<>0 union
	select t_id, t_busy3 rid, 3 AS p from tiers where t_busy3<>0 and t_busy3 is not null union
	select t_id, t_busy4 rid, 4 AS p from tiers where t_busy4<>0 and t_busy4 is not null
) bus 
where rid not in(select r_id from rabbits);