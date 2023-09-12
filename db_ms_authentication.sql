-- CREATE DATABASE dbprueba

--Tabla inicial de información usuario
/*create table Usuario(
	IdUsuario int primary key identity,
	NombreUsuario varchar (20),
	Clave varchar(20)
	)
*/
/*
	insert into Usuario (NombreUsuario, Clave)
	values
	('Admin', '123')
*/

--SELECT * FROM Usuario

--Tabla que almacena la información de los Tokens
/*create table HistorialRefreshToken(
	IdHistorialToken int primary key identity,
	IdUsuario int references Usuario(IdUsuario),
	Token varchar(500),
	RefreshToken varchar(200),
	FechaCreación datetime,
	FechaExpiracion datetime,
	EsActivo AS (iif(FechaExpiracion < getdate(), convert(bit,0), convert (bit,1))) -- Columna calculada
	)
*/	
/*
	insert into TABLA_PRUEBA(FechaCreacion, FechaExpiracion)
	values(
	GETDATE(),
	DATEADD(SECOND, 10, getdate()))
*/

--drop table TABLA_PRUEBA
