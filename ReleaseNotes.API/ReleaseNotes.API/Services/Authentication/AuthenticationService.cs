using ReleaseNotes.API.Data.Models;
using ReleaseNotes.API.Options;
using ReleaseNotes.API.Services.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReleaseNotes.API.Services.Authentication;

public class AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<JwtOptions> jwtOptions) : IAuthenticationService
{
    public JwtOptions JwtOptions { get; } = jwtOptions.Value;

    public async Task<LoginResponseModel> Login(LoginRequestModel loginRequestModel)
    {
        var user = await userManager.FindByEmailAsync(loginRequestModel.Email);

        if (user == null)
        {
            return new LoginResponseModel()
            {
                Success = false
            };
        }

        if (user.HasAccess == false)
        {
            return new LoginResponseModel()
            {
                Success = false
            };
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, loginRequestModel.Password, false);

        return new LoginResponseModel()
        {
            Success = result.Succeeded,
            Jwt = result.Succeeded ? GenerateJwtToken(user) : string.Empty
        };
    }

    public async Task<RegisterResponseModel> Register(RegisterRequestModel registerRequestModel)
    {
        var user = new User
        {
            UserName = registerRequestModel.Email,
            Email = registerRequestModel.Email,
            HasAccess = registerRequestModel.HasAccess,
        };

        var result = await userManager.CreateAsync(user, registerRequestModel.Password);
        return new RegisterResponseModel()
        {
            Success = result.Succeeded
        };
    }

    private string GenerateJwtToken(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new Exception("Email can not be null");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(JwtOptions.Secret);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
