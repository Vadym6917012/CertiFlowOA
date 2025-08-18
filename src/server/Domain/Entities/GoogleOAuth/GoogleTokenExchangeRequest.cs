using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.GoogleOAuth
{
    public class GoogleTokenExchangeRequest
    {
        public string AuthorizationCode { get; set; }
        public string CodeVerifier { get; set; }
    }
}
