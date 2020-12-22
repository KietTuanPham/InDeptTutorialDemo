﻿using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using InDeptTutorialDemo.Module.BusinessObjects;

namespace InDeptTutorialDemo.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            PermissionPolicyRole adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(
                new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = SecurityStrategy.AdministratorRoleName;
                adminRole.IsAdministrative = true;
            }

            PermissionPolicyRole userRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Users"));
            if (userRole == null)
            {
                userRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                userRole.Name = "Users";
                userRole.PermissionPolicy = SecurityPermissionPolicy.AllowAllByDefault;
                userRole.AddTypePermission<PermissionPolicyRole>(SecurityOperations.FullAccess, SecurityPermissionState.Deny);
                userRole.AddTypePermission<PermissionPolicyUser>(SecurityOperations.FullAccess, SecurityPermissionState.Deny);
                userRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.ReadOnlyAccess, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                userRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", null, SecurityPermissionState.Allow);
                userRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", null, SecurityPermissionState.Allow);
                userRole.AddTypePermission<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                userRole.AddTypePermission<PermissionPolicyTypePermissionObject>("Write;Delete;Navigate;Create", SecurityPermissionState.Deny);
                userRole.AddTypePermission<PermissionPolicyMemberPermissionsObject>("Write;Delete;Navigate;Create", SecurityPermissionState.Deny);
                userRole.AddTypePermission<PermissionPolicyObjectPermissionsObject>("Write;Delete;Navigate;Create", SecurityPermissionState.Deny);
            }
            // If a user named 'Sam' does not exist in the database, create this user.
            PermissionPolicyUser user1 = ObjectSpace.FindObject<PermissionPolicyUser>(
              new BinaryOperator("UserName", "Sam"));
            if (user1 == null)
            {
                user1 = ObjectSpace.CreateObject<PermissionPolicyUser>();
                user1.UserName = "Sam";
                // Set a password if the standard authentication type is used.
                user1.SetPassword("");
            }
            // If a user named 'John' does not exist in the database, create this user.
            PermissionPolicyUser user2 = ObjectSpace.FindObject<PermissionPolicyUser>(
                 new BinaryOperator("UserName", "John"));
            if (user2 == null)
            {
                user2 = ObjectSpace.CreateObject<PermissionPolicyUser>();
                user2.UserName = "John";
                // Set a password if the standard authentication type is used.
                user2.SetPassword("");
            }
            user1.Roles.Add(adminRole);
            user2.Roles.Add(userRole);
            ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

				defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return defaultRole;
        }
    }
}