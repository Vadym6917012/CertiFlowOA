using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class RoleAssignmentException : Exception
    {
        public RoleAssignmentException(string message) : base(message) { }
    }
}
