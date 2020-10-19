USE master;
GO

DROP DATABASE IF EXISTS HealthHistoryReport;
GO

CREATE DATABASE HealthHistoryReport;
GO

USE HealthHistoryReport
GO

BEGIN TRANSACTION


CREATE TABLE pet
(
pet_id				int					identity (1,1),
pet_name			varchar(15)			not null,
pet_type			varchar(10)			not null,
pet_age					int				    not null,
pet_owner_name			varchar(30)					not null,	

	constraint pk_pet primary key (pet_id),

)

CREATE TABLE procedures
(
procedure_name			varchar(10)		  not null,
procedure_id			int					identity(1,1),
procedure_date			DateTime			not null,
pet_id					int					not null,

	constraint pk_procedures primary key (procedure_id),
	constraint fk_pet_id foreign key (pet_id) references pet (pet_id)

)

CREATE TABLE visit
(
visitDate				DateTime			not null,
pet_id					int					not null,
procedure_id			int				not null,

		
		
		constraint pk_visit primary key (visitDate,pet_id,procedure_id),

		constraint fk_pet_id1 foreign key (pet_id) references pet (pet_id),
				constraint fk_procedure_id foreign key (procedure_id) references procedures (procedure_id),
)

COMMIT TRANSACTION