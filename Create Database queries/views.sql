use Notebook;

go
CREATE VIEW UserNotes( UserID, NoteTitle, NoteText )
As
	SELECT usr.id, note.title, note.text
	FROM Notes AS note JOIN Users AS usr ON note.writerID = usr.id;

go
CREATE VIEW NotesLog( Username, NoteTitle, LogTime )
As
	SELECT usr.username, note.title, log.time
	FROM Logs AS log JOIN Notes AS note ON note.id = log.noteID
		JOIN Users AS usr ON usr.id = log.writerID;

go
CREATE TRIGGER NewLog ON Logs
AFTER INSERT
AS
BEGIN
	INSERT INTO Logs( writerID, noteID, Logs.time)
	VALUES(
		( 
			SELECT writerID
			FROM inserted
		),
		(
			SELECT id
			FROM inserted
		),
		CURRENT_TIMESTAMP
	) 		
END

go
CREATE PROCEDURE NewUser
	@username Nvarchar(25), @password text
AS
	BEGIN TRY
		INSERT INTO Notebook.Users( username, password )
		VALUES( @username, @password ) 
	END TRY
	BEGIN CATCH
		SELECT 'ErorR'
	END CATCH

go 
CREATE FUNCTION UsersCount()
RETURNS INT
AS
	BEGIN
		RETURN (SELECT COUNT( id ) as itemCount FROM Users)
	END
