using RonWeb.API.Models.Login;
using RonWeb.API.Models.RefreshToken;
using RonWeb.API.Models.Shared;

namespace RonWeb.API.Interface.RefreshToken
{
    public interface IRefreshTokenHelper
    {
        public Task<Token> Refresh(RefreshTokenRequest data);
    }
}
