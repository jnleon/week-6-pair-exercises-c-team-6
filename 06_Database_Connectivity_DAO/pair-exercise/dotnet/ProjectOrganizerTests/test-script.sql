-- Delete all of the data
DELETE FROM project_employee ;
DELETE FROM employee;
DELETE FROM project ;
DELETE FROM department ;

-- Insert a fake department 
INSERT INTO department VALUES ('kroger');
DECLARE @newDepartmentId int = (SELECT @@IDENTITY);

-- Insert a fake project
INSERT INTO project VALUES ('project1', '2020-10-22','2020-10-23');
DECLARE @newProjectId int = (SELECT @@IDENTITY);

-- Insert a fake employee
INSERT INTO employee VALUES (@newDepartmentId,'pe', 'pe', 'Laborer', '2020-10-22','M', '2020-10-22');
DECLARE @newEmployeeId int = (SELECT @@IDENTITY);

-- Assign the fake employee to a fake project
INSERT INTO project_employee VALUES ( @newProjectId,@newEmployeeId);

-- Return the id of the fake dept, project & employee
SELECT @newDepartmentId as newDepartmentId ,
@newProjectId as newProjectId, 
@newEmployeeId as newEmployeeId;	

