
UPDATE options SET o_value='2' WHERE o_name='db' AND o_subname='version';
ALTER TABLE options MODIFY o_value TEXT NULL;
ALTER TABLE filters MODIFY f_filter TEXT NULL;
ALTER TABLE tiers MODIFY t_notes TEXT NULL;
ALTER TABLE rabbits MODIFY r_notes TEXT NULL;
ALTER TABLE fucks MODIFY f_notes TEXT NULL;
ALTER TABLE genezis MODIFY g_notes TEXT NULL;
ALTER TABLE dead MODIFY d_notes TEXT NULL;
ALTER TABLE dead MODIFY r_notes TEXT NULL;
ALTER TABLE logtypes MODIFY l_params TEXT NULL;
ALTER TABLE logs MODIFY l_param TEXT NULL;