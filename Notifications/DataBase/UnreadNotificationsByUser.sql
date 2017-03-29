ALTER proc [dbo].[Notifications_Unread_GetByUserId]
@userId nvarchar(128) 

AS

BEGIN

SELECT

 [id]
,[userId]
,[dateCreated]
,[dateUpdated]
,[category]
,[message1]
,[is_read]
,[link]
,[message2]
,[message3]



FROM
[dbo].[Notifications]
WHERE [userId] = @userId AND is_read = 0


END