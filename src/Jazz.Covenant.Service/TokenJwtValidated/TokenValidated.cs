using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Covenant.Service.TokenJwtValidated
{
    public class TokenValidated : ITokenValidated
    {
        public bool ValidateToken(string identificador, string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters =GetValidationParameters(identificador);
            try
            {

                tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
        private static TokenValidationParameters GetValidationParameters(string identificador) => new()
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidIssuer = "Sample",
            ValidAudience = "Sample",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identificador))
        };

    }
}
