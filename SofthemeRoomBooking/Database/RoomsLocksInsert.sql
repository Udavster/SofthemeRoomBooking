CREATE TRIGGER RoomsLocksInsert
ON [dbo].[RoomsLocks] AFTER INSERT 
AS

declare @start smalldatetime;
declare @finish smalldatetime;
declare @room int;

select @start = i.Start from inserted AS i;
select @finish = i.Finish from inserted AS i;
select @room = i.IdRoom from inserted AS i;

IF @finish IS NULL
	BEGIN
		
		UPDATE Events SET Events.Cancelled = 1 WHERE (Events.Id_room = @room) AND 
										((Events.Start>=@start) OR
										(Events.Finish>=@start) OR
										((Events.Start<=@start) AND (Events.Finish>=@start)))
	END
ELSE
	BEGIN
		UPDATE Events SET Events.Cancelled = 1 WHERE (Events.Id_room = @room) AND 
										(((Events.Start>=@start) AND (Events.Start<=@finish)) OR
										((Events.Finish>=@start) AND (Events.Finish<=@finish)) OR
										((Events.Start<=@start) AND (Events.Finish>=@start)))
	END
GO
