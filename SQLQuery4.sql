create database desafio_final

use desafio_final

create table roles
(
	Id int identity(1,1) not null,
	Nome varchar(30) not null,
	constraint PK_Id_Roles primary key (Id)
)

create table usuarios
(
	Id int identity(1,1) not null,
	RoleId int not null,
	Nome varchar(60) not null,
	Email varchar(255) not null,
	Senha varchar(60) not null,
	CriadoEm datetime not null,
	constraint PK_Id_Usuarios primary key (Id),
	constraint FK_RoledId_Usuarios foreign key (RoleId) references roles(Id)
)

create table autores
(
	Id int identity(1,1) not null,
	Nome varchar(60) not null,
	Sobrenome varchar(60) not null,
	CriadoEm datetime not null,
	constraint PK_Id_Autores primary key (Id)
)

create table livros
(
	Id int identity(1,1) not null,
	AutorId int not null,
	Descricao varchar(60) not null,
	ISBN int not null,
	AnoLancamento int not null,
	CriadoEm datetime not null,
	constraint PK_Id_Livros primary key (Id),
	constraint FK_AutorId_Livros foreign key (AutorId) references autores(Id)
)


