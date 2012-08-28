
INSERT INTO [Users]([UserId], [UserName], [AddressId])
VALUES(1,'user1',null)
INSERT INTO [Users]([UserId], [UserName], [AddressId])
VALUES(14,'user14',null)

INSERT INTO [Roles ]([RoleId], [RoleName])
VALUES(1,'Admin');
INSERT INTO [Roles ]([RoleId], [RoleName])
VALUES(2,'User');

INSERT INTO [Applications]([ApplicationId], [ApplicationName], [DefaultRoleId])
VALUES(1, 'Application Manager', 1);

INSERT INTO [ApplicationUserRoles]([ApplicationId], [UserId], [RoleId])
VALUES(1,1,2);
INSERT INTO [ApplicationUserRoles]([ApplicationId], [UserId], [RoleId])
VALUES(1,14,1);
INSERT INTO [ApplicationUserRoles]([ApplicationId], [UserId], [RoleId])
VALUES(1,14,2);

INSERT INTO [ApplicationUsers]([ApplicationId], [UserId], [ActiveFlag])
VALUES(1,1,1);
INSERT INTO [ApplicationUsers]([ApplicationId], [UserId], [ActiveFlag])
VALUES(1,14,1);