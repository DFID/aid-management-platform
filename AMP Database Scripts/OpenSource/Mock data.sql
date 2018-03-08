

-- These dummy users match the demo identity manager.

INSERT INTO [System].Users
(UserID, UserName, Logon, Status, LastUpdated, UserUpdated)
VALUES
('111111','A-Inputter','A Inputter','A',GETDATE(),'007007'),
('222222','A-SRO','An SRO','A',GETDATE(),'007007'),
('333333','A-Advisior','An Advisor','A',GETDATE(),'007007'),
('444444','A-TeamMember','A Team Member','A',GETDATE(),'007007'),
('555555','A-OfficeHead','A Office Head','A',GETDATE(),'007007')


INSERT INTO [System].UserLookUp
(ResourceID, UserName, UserLogon, LastUpdated, UserID)
VALUES
('111111','A-Inputter','A Inputter',GETDATE(),'007007'),
('222222','A-SRO','An SRO',GETDATE(),'007007'),
('333333','A-Advisior','An Advisor',GETDATE(),'007007'),
('444444','A-TeamMember','A Team Member',GETDATE(),'007007'),
('555555','A-OfficeHead','A Office Head',GETDATE(),'007007')

--Project data

SELECT * FROM System.BudgetCentre
SELECT * FROM PROJECT.ProjectMaster

UPDATE PROJECT.ProjectMaster SET BudgetCentreID = 'P0001'

update system.BudgetCentre
set BudgetCentreID = 'P0002'
WHERE BudgetCentreID = 'A0002'


select * from system.FundingMech
select * from system.FundingMechToSector