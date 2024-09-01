CREATE DATABASE Wallet4
GO
USE Wallet4
GO
CREATE TABLE Usuario(
	UsuarioId int primary key identity,
	Email varchar(25) not null,
	Clave varchar(8) not null,
	Saldo money not null,
	Estado int not null
)
CREATE TABLE Divisa(
	DivisaId int primary key identity,
	Nombre varchar(15) not  null,
	Valor money not null
)
CREATE TABLE MisMonedas(
	DivisaId int foreign key (DivisaId) references  Divisa(DivisaId) not null,
	UsuarioId int foreign key (UsuarioId) references Usuario(UsuarioId) not null,
	Saldo money not null
)

CREATE TABLE Historial(
	HistorialId int primary key identity,
	UsuarioId int foreign key (UsuarioId) references  Usuario(UsuarioId) not null,
	--DivisaId int foreign key (DivisaId) references  Divisa(DivisaId) not null,
	TipoTransaccion varchar(10) not null,
	Descripcion varchar(50) not null,
	Monto money not null,
	Cargo money not null,
	Total money not null,
	Fecha date not null
)