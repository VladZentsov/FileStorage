using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Validation
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string message) : base(message)
        {

        }
    }
}
