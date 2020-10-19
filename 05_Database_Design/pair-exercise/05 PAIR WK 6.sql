USE master;
GO

DROP DATABASE IF EXISTS AnimalHospital;
GO

CREATE DATABASE AnimalHospital;
GO

USE AnimalHospital
GO

BEGIN TRANSACTION


CREATE TABLE pet
(
pet_id				int					identity (1,1),
pet_name			varchar(15)			not null,
pet_type			varchar(10)			not null,
age					int				    not null,


	constraint pk_pet primary key (pet_id),
)

CREATE TABLE owner
(
owner_id			int				   identity(1,1),
first_name			varchar(15)			not null,
last_name			varchar(30)			not null,
address			    varchar(100)		not null,
city_sate			varchar(15)		   not null,
pet_id				int					not null,

	constraint pk_owner primary key (owner_id),
	constraint fk_pet foreign key (pet_id) references pet (pet_id)
)

CREATE TABLE petprocedure
(
procedure_id		int 			identity(1,1),
procedure_name		varchar(30)		not null,
procedure_price		decimal			not null,

	constraint pk_petprocedure primary key (procedure_id),

)

CREATE TABLE visit 
(
owner_id		int			not null,
pet_id			int			not null,	
procedure_id	int 		not null,
visit_date		Date		not null,


	constraint pk_visit primary key(owner_id,pet_id,procedure_id),
		constraint fk_petprocedure foreign key(procedure_id) references petprocedure (procedure_id),
			constraint fk_pet1 foreign key( pet_id) references pet (pet_id)

)
COMMIT TRANSACTION