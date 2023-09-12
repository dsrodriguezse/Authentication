using Authentication.Models.Custom;

namespace Authentication.Services
{
    public interface IAuthorizationService
    {

        Task <AuthorizationResponse>DevolverToken(AuthorizationRequest authorization);
        Task<AuthorizationResponse> DevolverRefreshToken(RefreshTokenRequest refreshTokenRequest, int idUsuario);
    }
}
