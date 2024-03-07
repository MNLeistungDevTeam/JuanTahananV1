CREATE TABLE [dbo].[Country] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Code]          NVARCHAR (50)  NULL,
    [Name]          NVARCHAR (100) NULL,
    [CurrencyCode]  NVARCHAR (50)  NULL,
    [FipsCode]      NVARCHAR (50)  NULL,
    [IsoNumber]     NVARCHAR (50)  NULL,
    [North]         NVARCHAR (50)  NULL,
    [South]         NVARCHAR (50)  NULL,
    [East]          NVARCHAR (50)  NULL,
    [West]          NVARCHAR (50)  NULL,
    [Capital]       NVARCHAR (50)  NULL,
    [ContinentName] NVARCHAR (100) NULL,
    [Continent]     NVARCHAR (50)  NULL,
    [Languages]     NVARCHAR (100) NULL,
    [IsoAlpha3]     NVARCHAR (50)  NULL,
    [GeoNameId]     INT            NULL,
    [CreatedById]   INT            NULL,
    [DateCreated]   DATETIME2 (7)  NULL,
    [ModifiedById]  INT            NULL,
    [DateModified]  DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([Id] ASC)
);



 
GO

CREATE TRIGGER [dbo].[Trigger_Country_ColumnUpdates] ON [dbo].[Country] --Change to match your table name
   FOR INSERT, UPDATE, DELETE
AS
SET NOCOUNT ON;

DECLARE @sql VARCHAR(5000) ,
    @sqlInserted NVARCHAR(500) ,
    @sqlDeleted NVARCHAR(500) ,
    @NewValue NVARCHAR(100) ,
    @OldValue NVARCHAR(100) ,
    @UpdatedBy VARCHAR(50) ,
    @ParmDefinitionD NVARCHAR(500) ,
    @ParmDefinitionI NVARCHAR(500) ,
    @TABLE_NAME VARCHAR(100) ,
    @COLUMN_NAME VARCHAR(100) ,
    @modifiedColumnsList NVARCHAR(4000) ,
    @ColumnListItem NVARCHAR(500) ,
    @Pos INT ,
    @RecordPk VARCHAR(50) ,
    @RecordPkName VARCHAR(50);

SELECT  *
INTO    #deleted
FROM    deleted;
SELECT  *
INTO    #Inserted
FROM    inserted;

DECLARE @Action as NVARCHAR(10);
SET @Action = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                        AND EXISTS(SELECT * FROM DELETED)
                    THEN 'Update'  -- Set Action to Updated.
                    WHEN EXISTS(SELECT * FROM INSERTED)
                    THEN 'Create'  -- Set Action to Insert.
                    WHEN EXISTS(SELECT * FROM DELETED)
                    THEN 'Delete'  -- Set Action to Deleted.
                    ELSE NULL -- Skip. It may have been a "failed delete".   
                END)

SET @TABLE_NAME = 'Country'; ---Change to your table name
IF(@Action = 'Create')
BEGIN
    SELECT  @UpdatedBy = CreatedById --Change to your column name for the user update field
    FROM    inserted;
END
ELSE
BEGIN
    SELECT  @UpdatedBy = ModifiedById --Change to your column name for the user update field
    FROM    inserted;
END
SELECT  @RecordPk = Id --Change to the table primary key field
FROM    inserted;   
SET @RecordPkName = 'Id';
SET @modifiedColumnsList = STUFF(( SELECT   ',' + name
                                   FROM     sys.columns
                                   WHERE    object_id = OBJECT_ID(@TABLE_NAME)
                                            AND SUBSTRING(COLUMNS_UPDATED(),
                                                          ( ( column_id
                                                          - 1 ) / 8 + 1 ),
                                                          1) & ( POWER(2,
                                                          ( ( column_id
                                                          - 1 ) % 8 + 1 )
                                                          - 1) ) = POWER(2,
                                                          ( column_id - 1 )
                                                          % 8)
                                 FOR
                                   XML PATH('')
                                 ), 1, 1, '');


WHILE LEN(@modifiedColumnsList) > 0
    BEGIN
        SET @Pos = CHARINDEX(',', @modifiedColumnsList);
        IF @Pos = 0
            BEGIN
                SET @ColumnListItem = @modifiedColumnsList;
            END;
        ELSE
            BEGIN
                SET @ColumnListItem = SUBSTRING(@modifiedColumnsList, 1,
                                                @Pos - 1);
            END;    

        SET @COLUMN_NAME = @ColumnListItem;
        SET @ParmDefinitionD = N'@OldValueOut NVARCHAR(100) OUTPUT';
        SET @ParmDefinitionI = N'@NewValueOut NVARCHAR(100) OUTPUT';
        SET @sqlDeleted = N'SELECT @OldValueOut=' + @COLUMN_NAME
            + ' FROM #deleted where ' + @RecordPkName + '='
            + CONVERT(VARCHAR(50), @RecordPk);
        SET @sqlInserted = N'SELECT @NewValueOut=' + @COLUMN_NAME
            + ' FROM #Inserted where ' + @RecordPkName + '='
            + CONVERT(VARCHAR(50), @RecordPk);
        EXECUTE sp_executesql @sqlDeleted, @ParmDefinitionD,
            @OldValueOut = @OldValue OUTPUT;
        EXECUTE sp_executesql @sqlInserted, @ParmDefinitionI,
            @NewValueOut = @NewValue OUTPUT;
        IF ( LTRIM(RTRIM(@NewValue)) != LTRIM(RTRIM(@OldValue)) )
            BEGIN   
                SET @sql = 'INSERT INTO [dbo].[AuditTrail]
                                               ([TableName]
                                               ,[RecordPK]
                                               ,[Action]
                                               ,[ColumnName]
                                               ,[OldValue]
                                               ,[NewValue]
                                               ,[UserId])
                                         VALUES
                                               (' + QUOTENAME(@TABLE_NAME, '''') + '
                                               ,' + QUOTENAME(@RecordPk, '''') + '
                                               ,' + QUOTENAME(@Action, '''') + '
                                               ,' + QUOTENAME(@COLUMN_NAME, '''') + '
                                               ,' + QUOTENAME(@OldValue, '''') + '
                                               ,' + QUOTENAME(@NewValue, '''') + '
                                               ,' + QUOTENAME(@UpdatedBy, '''') + ')';


                EXEC (@sql);
            END;     
        SET @COLUMN_NAME = '';
        SET @NewValue = '';
        SET @OldValue = '';
        IF @Pos = 0
            BEGIN
                SET @modifiedColumnsList = '';
            END;
        ELSE
            BEGIN
           -- start substring at the character after the first comma
                SET @modifiedColumnsList = SUBSTRING(@modifiedColumnsList,
                                                     @Pos + 1,
                                                     LEN(@modifiedColumnsList)
                                                     - @Pos);
            END;
    END;
DROP TABLE #Inserted;
DROP TABLE #deleted;