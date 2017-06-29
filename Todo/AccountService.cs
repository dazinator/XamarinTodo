using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Todo
{
   

    public interface IAccountService
    {
        Task AddAccount(string accountType, string authTokenType);
    }
}
