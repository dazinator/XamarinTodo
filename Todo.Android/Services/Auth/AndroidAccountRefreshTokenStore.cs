using Gluon.Client.Jwt;
using System.Threading.Tasks;

namespace Todo
{
    public class AndroidAccountRefreshTokenStore : IRefreshTokenStore
    {
        private IAccountService _accountService;

        public AndroidAccountRefreshTokenStore(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<string> LoadRefreshToken()
        {
            var currentAccount = _accountService.CurrentUserAccount;
            var result = await _accountService.GetRefreshToken(currentAccount);
            return result;

        }

        public async Task SaveRefreshToken(string token)
        {
            var currentAccount = _accountService.CurrentUserAccount;
            await _accountService.SetRefreshToken(currentAccount, token);
        }

    }
}


