using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Service.ManagerVPS.DTO.OtherModels;

namespace Service.ManagerVPS.Extensions.StaticLogic;

public static class JwtTokenExtension
{
    public static string WriteToken(UserTokenHeader userTokenInfo)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var claims = new[]
        {
            new Claim(nameof(UserTokenHeader.UserId), userTokenInfo.UserId),
            new Claim(nameof(UserTokenHeader.FirstName), userTokenInfo.FirstName),
            new Claim(nameof(UserTokenHeader.LastName), userTokenInfo.LastName),
            new Claim(nameof(UserTokenHeader.Email), userTokenInfo.Email),
            new Claim(nameof(UserTokenHeader.RoleId), userTokenInfo.RoleId.ToString()),
            new Claim(nameof(UserTokenHeader.RoleName), userTokenInfo.RoleName),
            new Claim(nameof(UserTokenHeader.Avatar), userTokenInfo.Avatar ?? string.Empty),
            new Claim(nameof(UserTokenHeader.Expires),
                userTokenInfo.Expires.ToString(CultureInfo.CurrentCulture)),
            new Claim(nameof(UserTokenHeader.ModifiedAt),
                userTokenInfo.ModifiedAt.ToString(CultureInfo.CurrentCulture))
        };

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: config["jwt:Issuer"],
            audience: config["jwt:Issuer"],
            expires: userTokenInfo.Expires,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["jwt:key"])),
                SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static UserTokenHeader? ReadToken(string tokenString)
    {
        try
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["jwt:key"])),
                ValidIssuer = config["jwt:Issuer"],
                ValidAudience = config["jwt:Issuer"],
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
            var handler = new JwtSecurityTokenHandler();
            var claimsPrincipal = handler.ValidateToken(tokenString, tokenValidationParameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken)
            {
                return null;
            }

            var userToken = GeneralExtension.ConvertClaimToUserToken(jwtToken.Claims);

            return userToken;
        }
        catch
        {
            return null;
        }
    }
}