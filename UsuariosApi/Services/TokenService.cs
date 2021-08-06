using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class TokenService
    {
        readonly SecuritySettings security;

        public TokenService(IOptionsMonitor<SecuritySettings> settings)
        {
            security = settings.CurrentValue;
        }

        public Task<Token> CreateTokenAsync(IdentityUser<int> usuario)
        {
            Claim[] direitosUsuario = new Claim[]
            {
                new Claim("username", usuario.UserName),
                new Claim("id", usuario.Id.ToString())
            };
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0asdjas09djsa09djasdjsadajsd09asjd09sajcnzxn"));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: security.Issuer,
                audience: security.Audience,
                claims: direitosUsuario,
                signingCredentials: credenciais,
                expires: DateTime.UtcNow.AddHours(security.HoursToExpire)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(new Token(tokenString));
        }
    }
}
