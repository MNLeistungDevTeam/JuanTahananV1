


IF NOT EXISTS (SELECT 1 FROM [dbo].Industry)
BEGIN
 SET IDENTITY_INSERT Industry ON

INSERT INTO Industry (Id, [Name], CreatedById, DateCreated, ModifiedById, DateModified)
VALUES 
(1, 'Accounting', 1, '2024-03-21T08:00:00', NULL, NULL),
(2, 'Activities of Private Households as Employers & Undifferentiated Production Activities of Private HouseHolds', 1, '2024-03-21T08:10:00', NULL, NULL),
(3, 'Agriculture, Hunting, Forestry & Fishing', 1, '2024-03-21T08:20:00', NULL, NULL),
(4, 'Basic Materials', 1, '2024-03-21T08:30:00', NULL, NULL),
(5, 'Business Process Outsourcing (BPO)', 1, '2024-03-21T08:40:00', NULL, NULL),
(6, 'Construction', 1, '2024-03-21T08:50:00', NULL, NULL),
(7, 'Education & Training', 1, '2024-03-21T09:00:00', NULL, NULL),
(8, 'Electricity, Gas and Water Supply', 1, '2024-03-21T09:10:00', NULL, NULL),
(9, 'Extra-Territorial Organization & Bodies', 1, '2024-03-21T09:20:00', NULL, NULL),
(10, 'Financial Services Intermediation', 1, '2024-03-21T09:30:00', NULL, NULL),
(11, 'HR/Recruitment', 1, '2024-03-21T09:40:00', NULL, NULL),
(12, 'Health and Social Work; Health and Medical Services', 1, '2024-03-21T09:50:00', NULL, NULL),
(13, 'Life Sciences', 1, '2024-03-21T10:00:00', NULL, NULL),
(14, 'Management', 1, '2024-03-21T10:10:00', NULL, NULL),
(15, 'Manufacturing', 1, '2024-03-21T10:20:00', NULL, NULL),
(16, 'Media', 1, '2024-03-21T10:30:00', NULL, NULL),
(17, 'Mining and Quarrying', 1, '2024-03-21T10:40:00', NULL, NULL),
(18, 'Other Community, Social & Personal Service Activities', 1, '2024-03-21T10:50:00', NULL, NULL),
(19, 'Public Administration & Defense; Compulsory Social Security', 1, '2024-03-21T11:00:00', NULL, NULL),
(20, 'Technology', 1, '2024-03-21T11:10:00', NULL, NULL),
(21, 'Transport, Storage and Communications', 1, '2024-03-21T11:20:00', NULL, NULL),
(22, 'Travel and Leisure', 1, '2024-03-21T11:30:00', NULL, NULL),
(23, 'Wholesale & Retail Trade; Repair of Motor Vehicles, Motorcycles, Personal & Household Goods', 1, '2024-03-21T11:40:00', NULL, NULL);

SET IDENTITY_INSERT Industry OFF
END
GO