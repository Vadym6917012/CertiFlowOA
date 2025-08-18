using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class PasswordChangeException : Exception
    {
        public PasswordChangeException(string message) : base(message) { }
    }
}
