UPDATE options SET o_value='6' WHERE o_name='db' AND o_subname='version';

#DELIMITER |
DROP FUNCTION IF EXISTS rabplace |
CREATE FUNCTION rabplace(rid INTEGER UNSIGNED) RETURNS char(150)
BEGIN
  DECLARE res VARCHAR(150);
  DECLARE i1,i2,i3,tid,s1,s2,s3 VARCHAR(20);
  DECLARE par INTEGER UNSIGNED;
  SELECT r_farm,r_tier_id,r_area,r_tier,r_parent
  INTO i1,i2,i3,tid,par
  FROM rabbits WHERE r_id=rid;
  IF (par<>0) THEN
	SELECT r_farm,r_tier_id,r_area,r_tier
	INTO i1,i2,i3,tid
	FROM rabbits WHERE r_id=par;
  END IF;
  IF (tid=0) THEN
    RETURN('');
  END IF;
  SELECT t_type,t_delims,t_nest INTO s1,s2,s3 FROM tiers WHERE t_id=tid;
  IF (ISNULL(s1)) THEN
    RETURN('');
  END IF;
  SET res=CONCAT_WS(',',i1,i2,i3,s1,s2,s3);
  RETURN(res);
END |

DROP FUNCTION IF EXISTS deadplace |
CREATE FUNCTION deadplace(rid INTEGER UNSIGNED) RETURNS char(150)
BEGIN
  DECLARE res VARCHAR(150);
  DECLARE i1,i2,i3,tid,s1,s2,s3 VARCHAR(20);
  SELECT r_farm,r_tier_id,r_area,r_tier
  INTO i1,i2,i3,tid
  FROM dead WHERE r_id=rid;
  IF (tid=0 AND i1=0) THEN
    RETURN('');
  END IF;
  IF (tid<>0) THEN
	SELECT t_type,t_delims,t_nest INTO s1,s2,s3 FROM tiers WHERE t_id=tid;
  ELSE
	SELECT t_type,t_delims,t_nest INTO s1,s2,s3 FROM tiers,minifarms WHERE m_id=i1 AND ((t_id=m_upper AND i2<>1) OR (t_id=m_lower AND i2=1));
  END IF;
  IF (ISNULL(s1)) THEN
    RETURN('');
  END IF;
  SET res=CONCAT_WS(',',i1,i2,i3,s1,s2,s3);
  RETURN(res);
END |		
#DELIMITER ;
