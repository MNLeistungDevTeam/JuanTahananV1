IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormatBytes]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
    EXEC('CREATE FUNCTION [dbo].[FormatBytes]
    (
        @bytes BIGINT
    )
    RETURNS NVARCHAR(20)
    AS
    BEGIN
        DECLARE @suffixes NVARCHAR(10) = ''BKMGTP'';
        DECLARE @index INT = 1;
        DECLARE @size DECIMAL(18, 2) = @bytes;
        
        WHILE (@size >= 1024 AND @index < LEN(@suffixes))
        BEGIN
            SET @size = @size / 1024;
            SET @index = @index + 1;
        END
        
        RETURN CONCAT(ROUND(@size, 2), '' '', SUBSTRING(@suffixes, @index, 1) + ''B'');
    END;');
END;
