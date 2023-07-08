/* PK KILLS ADD COLUMN */
use ace_pk_kills;
ALTER TABLE kills ADD COLUMN kill_Type varchar(255) DEFAULT "GLOBAL";
