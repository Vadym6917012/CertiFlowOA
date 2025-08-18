using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class GoogleAuthException : Exception
    {
        public GoogleAuthException(string message) : base(message) { }
    }
}
