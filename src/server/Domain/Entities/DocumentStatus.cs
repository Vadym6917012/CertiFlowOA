using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum DocumentStatus
    {
        New = 1,
        InReview = 2,
        Signed = 3,
        Rejected = 4,
        Completed = 5
    }
}
