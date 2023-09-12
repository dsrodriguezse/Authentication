using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Models;
using Authentication.Models.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Cryptography;

namespace Authentication.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly DbpruebaContext _context;
        private readonly IConfiguration _configuration;
        public AuthorizationService(DbpruebaContext context, IConfiguration configuration){
            _context = context;
            _configuration = configuration;
        }

        private string GenerarToken(string idUsuario){
            var key = _configuration.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));

            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credencialesToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;
        }

        private string GenerarRefreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteArray)
;
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }

        private async Task<AuthorizationResponse>GuardarHistorialRefreshToken(
            int idUsuario,
            string token,
            string refreshToken
            ){

            var historialRefreshToken = new HistorialRefreshToken
            {
                IdUsuario = idUsuario,
                Token = token,
                RefreshToken = refreshToken,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(2)
            };

            await _context.HistorialRefreshTokens.AddAsync(historialRefreshToken);
            await _context.SaveChangesAsync();

            return new AuthorizationResponse { Token = token, RefreshToken = refreshToken, Resultado = true, Msg = "Ok" };
        }


        public async Task<AuthorizationResponse> DevolverToken(AuthorizationRequest authorization)
        {
            var usuario_encontrado = _context.Usuarios.FirstOrDefault(x =>
                x.NombreUsuario == authorization.NombreUsuario &&
                x.Clave == authorization.Clave
            );

            if(usuario_encontrado == null){
                return await Task.FromResult<AuthorizationResponse>(null);
            }
            
            string tokenCreado = GenerarToken(usuario_encontrado.IdUsuario.ToString());

            string refreshTokenCreado = GenerarRefreshToken();

            /*
             * return new AuthorizationResponse(){
                Token = tokenCreado,
                Resultado = true,
                Msg = "Ok"
            };
            */
            return await GuardarHistorialRefreshToken(usuario_encontrado.IdUsuario, tokenCreado, refreshTokenCreado);
        }

        public async Task<AuthorizationResponse> DevolverRefreshToken(RefreshTokenRequest refreshTokenRequest, int idUsuario)
        {
            var refreshTokenEncontrado = _context.HistorialRefreshTokens.FirstOrDefault(x =>
            x.Token == refreshTokenRequest.TokenExpirado &&
            x.RefreshToken == refreshTokenRequest.RefreshToken &&
            x.IdUsuario == idUsuario);

            if (refreshTokenEncontrado == null){
                return new AuthorizationResponse { Resultado = false, Msg = "No existe refreshToken" };
            }

            var refreshTokenCreado = GenerarRefreshToken();
            var tokenCreado = GenerarToken(idUsuario.ToString());

            return await GuardarHistorialRefreshToken(idUsuario, tokenCreado, refreshTokenCreado);
        
        }
    }
}
