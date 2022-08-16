namespace HRSystem.Constants
{
    public static class Permissions
    {
        public static List<string> generatePermissionsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }
        public static List<string> generateAllPermissions()
        {
            var allPermissions = new List<string>();
            var modules = Enum.GetValues(typeof(Modules));
            foreach (var module in modules)
            {
                allPermissions.AddRange(generatePermissionsList(module.ToString()));
            }
            return allPermissions;
        }
        public static class Attendance
        {
            public const string View = "Permissions.Attendance.View";
            public const string Create = "Permissions.Attendance.Create";
            public const string Edit = "Permissions.Attendance.Edit";
            public const string Delete = "Permissions.Attendance.Delete";
        }
        public static class Employee
        {
            public const string View = "Permissions.Employee.View";
            public const string Create = "Permissions.Employee.Create";
            public const string Edit = "Permissions.Employee.Edit";
            public const string Delete = "Permissions.Employee.Delete";
        }
        public static class GeneralSettings
        {
            public const string View = "Permissions.GeneralSettings.View";
            public const string Create = "Permissions.GeneralSettings.Create";
            public const string Edit = "Permissions.GeneralSettings.Edit";
            public const string Delete = "Permissions.GeneralSettings.Delete";
        }
        public static class Holidays
        {
            public const string View = "Permissions.Holidays.View";
            public const string Create = "Permissions.Holidays.Create";
            public const string Edit = "Permissions.Holidays.Edit";
            public const string Delete = "Permissions.Holidays.Delete";
        }
        public static class Home
        {
            public const string View = "Permissions.Home.View";
            public const string Create = "Permissions.Home.Create";
            public const string Edit = "Permissions.Home.Edit";
            public const string Delete = "Permissions.Home.Delete";
        }
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }
        public static class SalaryReport
        {
            public const string View = "Permissions.SalaryReport.View";
            public const string Create = "Permissions.SalaryReport.Create";
            public const string Edit = "Permissions.SalaryReport.Edit";
            public const string Delete = "Permissions.SalaryReport.Delete";
        }
         public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

    }
    

    
}
