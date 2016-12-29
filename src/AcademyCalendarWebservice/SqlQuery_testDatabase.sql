--create database Frank
go
use Frank
go

create table Room (
Id int identity primary key,
Name varchar(64) unique not null,
Capacity int not null,
Has_WhiteBoard bit null,
Has_Projector bit null,
Has_TvScreen bit null
)
go

create table Occupant(
Id int identity primary key,
FirstName varchar(64) not null,
LastName varchar(64) not null,
Email varchar(256) not null,
UserRole varchar(64) not null
)
go

create table Booking(
    Id int identity primary key,
    StartTime DateTime not null,
    EndTime DateTime not null,
    Room_Id int not null constraint FK_BookingRoom foreign key references Room(Id),
    Occupant_Id int not null constraint FK_BookingOccupant foreign key references Occupant(Id),
    Title varchar(64) not null,
    Decription varchar(max) null,
)
go

sp_rename 'Frank.Decription', 'Description', 'COLUMN';
go