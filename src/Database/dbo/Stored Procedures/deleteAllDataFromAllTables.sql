CREATE PROCEDURE [dbo].[deleteAllDataFromAllTables] AS
SET NOCOUNT ON

    DECLARE @dropAndCreateConstraintsTable TABLE ( DropStmt varchar(max) , CreateStmt varchar(max) )

    insert @dropAndCreateConstraintsTable select
      DropStmt = 'ALTER TABLE [' + ForeignKeys.ForeignTableSchema + 
          '].[' + ForeignKeys.ForeignTableName + 
          '] DROP CONSTRAINT [' + ForeignKeys.ForeignKeyName + ']; '
    ,  CreateStmt = 'ALTER TABLE [' + ForeignKeys.ForeignTableSchema + 
          '].[' + ForeignKeys.ForeignTableName + 
          '] WITH CHECK ADD CONSTRAINT [' +  ForeignKeys.ForeignKeyName + 
          '] FOREIGN KEY([' + ForeignKeys.ForeignTableColumn + 
          ']) REFERENCES [' + schema_name(sys.objects.schema_id) + '].[' +
      sys.objects.[name] + ']([' +
      sys.columns.[name] + ']); '
     from sys.objects
      inner join sys.columns
        on (sys.columns.[object_id] = sys.objects.[object_id])
      inner join (
        select sys.foreign_keys.[name] as ForeignKeyName
         ,schema_name(sys.objects.schema_id) as ForeignTableSchema
         ,sys.objects.[name] as ForeignTableName
         ,sys.columns.[name]  as ForeignTableColumn
         ,sys.foreign_keys.referenced_object_id as referenced_object_id
         ,sys.foreign_key_columns.referenced_column_id as referenced_column_id
         from sys.foreign_keys
          inner join sys.foreign_key_columns
            on (sys.foreign_key_columns.constraint_object_id
              = sys.foreign_keys.[object_id])
          inner join sys.objects
            on (sys.objects.[object_id]
              = sys.foreign_keys.parent_object_id)
            inner join sys.columns
              on (sys.columns.[object_id]
                = sys.objects.[object_id])
               and (sys.columns.column_id
                = sys.foreign_key_columns.parent_column_id)
        ) ForeignKeys
        on (ForeignKeys.referenced_object_id = sys.objects.[object_id])
         and (ForeignKeys.referenced_column_id = sys.columns.column_id)
     where (sys.objects.[type] = 'U')
      and (sys.objects.[name] not in ('sysdiagrams'))


    DECLARE @DropStatement nvarchar(max)
    DECLARE @RecreateStatement nvarchar(max)

    DECLARE C1 CURSOR READ_ONLY
    FOR
    SELECT DropStmt from @dropAndCreateConstraintsTable
    OPEN C1

    FETCH NEXT FROM C1 INTO @DropStatement

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Executing ' + @DropStatement
        execute sp_executesql @DropStatement
        FETCH NEXT FROM C1 INTO @DropStatement
    END
    CLOSE C1
    DEALLOCATE C1

    DECLARE @DeleteTableStatement nvarchar(max)
    DECLARE C2 CURSOR READ_ONLY
    FOR
    SELECT 'Delete From [dbo].[' + TABLE_NAME + ']' from INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_SCHEMA = 'dbo' -- Change your schema appropriately.
    OPEN C2

    FETCH NEXT FROM C2 INTO @DeleteTableStatement

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Executing ' + @DeleteTableStatement
        execute sp_executesql @DeleteTableStatement
        FETCH NEXT FROM C2 INTO  @DeleteTableStatement
    END
    CLOSE C2
    DEALLOCATE C2

    DECLARE C3 CURSOR READ_ONLY
    FOR
    SELECT CreateStmt from @dropAndCreateConstraintsTable
    OPEN C3

    FETCH NEXT FROM C3 INTO @RecreateStatement

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Executing ' + @RecreateStatement
        execute sp_executesql @RecreateStatement
        FETCH NEXT FROM C3 INTO  @RecreateStatement
    END
    CLOSE C3
    DEALLOCATE C3