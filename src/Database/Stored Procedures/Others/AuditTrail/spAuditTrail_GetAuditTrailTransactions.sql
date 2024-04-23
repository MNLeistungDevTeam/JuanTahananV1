 CREATE PROCEDURE spAuditTrail_GetAuditTrailTransactions
    @transactionNo NVARCHAR(255)  
AS
BEGIN
    SELECT   
        at.Id,
        at.[Action],
        at.TableName,
        at.RecordPk,
        at.ColumnName,
        at.OldValue,
        at.NewValue,
        CONCAT(u.FirstName, ' ', u.LastName) AS FullName,
        CONVERT(varchar(19), at.ChangeDate, 120) AS ChangeDate 
    FROM 
        AuditTrail at
    LEFT JOIN 
        [User] u ON u.Id = at.UserId
    --WHERE 
    --    EXISTS (
    --        SELECT 1
    --        FROM (
    --            SELECT 'PurchaseRequest' AS TableName, Id FROM PurchaseRequest WHERE TransactionNo = @transactionNo
    --            UNION ALL
    --            SELECT 'PurchaseOrder' AS TableName, Id FROM PurchaseOrder WHERE TransactionNo = @transactionNo
    --            UNION ALL
    --            SELECT 'Quotation' AS TableName, Id FROM Quotation WHERE TransactionNo = @transactionNo
    --            UNION ALL
    --            SELECT 'Canvass' AS TableName, Id FROM Canvass WHERE TransactionNo = @transactionNo
    --            UNION ALL
    --            SELECT 'Billing' AS TableName, Id FROM Billing WHERE InvoiceNo = @transactionNo
    --            UNION ALL
    --            SELECT 'Payment' AS TableName, Id FROM Payment WHERE TransactionNo = @transactionNo
    --        ) AS TransactionRecords
    --        WHERE TransactionRecords.TableName = at.TableName AND TransactionRecords.Id = at.RecordPk
    --    )
    ORDER BY at.ChangeDate DESC;

    RETURN 0;
END
