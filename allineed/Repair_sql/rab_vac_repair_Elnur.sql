CREATE TEMPORARY TABLE aaa (
	SELECT r.r_id,v_id,`date` FROM rabbits r
	INNER JOIN rab_vac rv ON rv.r_id=r.r_id AND v_id=1 AND `date`<r_born
);

DELETE FROM rab_vac WHERE (r_id,v_id,`date`) in (select r_id,v_id,`date` FROM aaa);