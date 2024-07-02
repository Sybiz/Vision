IF NOT EXISTS (SELECT NULL FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'example')
	EXEC sys.sp_executesql N'CREATE SCHEMA [example] AUTHORIZATION [dbo]'
GO

IF NOT EXISTS(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ContactCategory' AND TABLE_SCHEMA = 'example')
BEGIN
	CREATE TABLE [example].[ContactCategory](
		[ContactId] [INT] NOT NULL,
		[Category] [NVARCHAR](50) NOT NULL
	) ON [PRIMARY]

	ALTER TABLE [example].[ContactCategory]  WITH CHECK ADD  CONSTRAINT [FK_ContactCategory_Contact] FOREIGN KEY([ContactId])
	REFERENCES [dr].[Contact] ([ContactId])
	ON DELETE CASCADE

	ALTER TABLE [example].[ContactCategory] CHECK CONSTRAINT [FK_ContactCategory_Contact]
END
GO

CREATE OR ALTER PROCEDURE [example].[mergeContactCategory]
(
	@ContactId INT,
	@Category NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT NULL FROM [example].[ContactCategory] WHERE ContactId = @ContactId)
		INSERT INTO [example].[ContactCategory] ([ContactId], [Category])
		VALUES (@ContactId, @Category)
	ELSE
		UPDATE [example].[ContactCategory] SET Category = @Category WHERE ContactId = @ContactId

END
GO


