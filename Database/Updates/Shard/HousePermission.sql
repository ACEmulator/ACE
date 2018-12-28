drop table if exists house_permission;

create table house_permission
(
	`id` int(10) unsigned not null auto_increment,
	`house_Id` int(10) unsigned not null,
	`player_Guid` int(10) unsigned not null,
	`storage` bit(1) not null,
	primary key (`id`),
	unique key `house_Id_player_Guid_uidx` (`house_Id`, `player_Guid`),
	key `house_Id_idx` (`house_Id`)
	constraint `house_Id` foreign key (`object_Id`) references `biota` (`id`) on delete cascade
);