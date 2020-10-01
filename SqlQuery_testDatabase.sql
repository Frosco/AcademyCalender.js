CREATE TABLE [dbo].[Booking]
 (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [StartingTime]   DATETIME      NOT NULL,
    [EndingTime]     DATETIME      NOT NULL,
    [Room_Id]     INT           NOT NULL,
    [Occupant_Id] INT           NOT NULL,
    [Title]       VARCHAR (64)  NOT NULL,
    [Description]  VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BookingRoom] FOREIGN KEY ([Room_Id]) REFERENCES [dbo].[Room] ([Id]),
    CONSTRAINT [FK_BookingOccupant] FOREIGN KEY ([Occupant_Id]) REFERENCES [dbo].[Occupant] ([Id])
);

