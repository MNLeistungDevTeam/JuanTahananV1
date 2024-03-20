

IF NOT EXISTS (SELECT 1 FROM [dbo].SourcePagibigFund)
BEGIN
-- Enable identity insert
SET IDENTITY_INSERT SourcePagibigFund ON

-- Insert 12 items
INSERT INTO SourcePagibigFund (Id, [Name], DateCreated, CreatedById, DateModified, ModifiedById)
VALUES
    (1, 'Tv Ad', GETDATE(), 1, GETDATE(), 1),
    (2, 'Radio Ad', GETDATE(), 1, GETDATE(), 1),
    (3, 'Pag-IBIG Fund Personnel', GETDATE(), 1, GETDATE(), 1),
    (4, 'Flyer/Poster/Brochure', GETDATE(), 1, GETDATE(), 1),
    (5, 'Employer', GETDATE(), 1, GETDATE(), 1),
    (6, 'Newspaper/Magazine Ad', GETDATE(), 1, GETDATE(), 1),
    (7, 'Website', GETDATE(), 1, GETDATE(), 1),
    (8, 'Agency', GETDATE(), 1, GETDATE(), 1),
    (9, 'Pag-IBIG Fund Branch', GETDATE(), 1, GETDATE(), 1),
    (10, 'Real Estate Developer', GETDATE(), 1, GETDATE(), 1),
    (11, 'Seller of the Property', GETDATE(), 1, GETDATE(), 1),
    (12, 'Others', GETDATE(), 1, GETDATE(), 1)

-- Disable identity insert
SET IDENTITY_INSERT SourcePagibigFund OFF
END
GO
