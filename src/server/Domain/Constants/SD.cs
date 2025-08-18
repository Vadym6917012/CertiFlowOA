using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class SD
    {
        // Roles
        public const string StudentRole = "Student";
        public const string FacultyCuratorRole = "FacultyCurator"; // Адміністрування заявок студентів
        public const string FacultyAdminRole = "FacultyAdmin"; // Створення довідок і управління студентами
        public const string SystemAdminRole = "SystemAdmin"; // налаштування системи й управління користувачами
    }
}
