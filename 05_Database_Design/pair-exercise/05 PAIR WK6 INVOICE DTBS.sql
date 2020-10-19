USE master;
GO

DROP DATABASE IF EXISTS INVOICE_PAIR;
GO

CREATE DATABASE INVOICE_PAIR;
GO

USE INVOICE_PAIR 
GO

BEGIN TRANSACTION


CREATE TABLE customer
(
pet_name			varchar(15)			not null,
customer_address	varchar(10)			not null,
customer_name		varchar(10)			not null,
customer_id				int					not null,

	constraint pk_customer_id primary key (customer_id),

)

CREATE TABLE invoice
(
procedure_name			varchar(10)		  not null,
invoice_id				int					identity(1,1),
procedure_date			DateTime			not null,
customer_id				int					not null,
procedure_price			decimal				not null,
	
		constraint pk_invoice_id primary key (invoice_id),

	
	constraint fk_customer_id foreign key (customer_id) references customer (customer_id)
)

COMMIT TRANSACTION