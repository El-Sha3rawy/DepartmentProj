using Auth.Proto;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiLayer.Services
{
    public class AuthGrpcService : AuthService.AuthServiceBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        IConfiguration _config;

        public AuthGrpcService (SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public override async Task <LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    return new LoginResponse { Success = false, Message = "Invalid Email or Password" };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    var token = GenerateJwtToken(user);

                    return new LoginResponse
                    {
                        Success = true,
                        Message = "Login Successful",
                        Token = token,
                        UserId = user.Id
                    };
                }
                return new LoginResponse { Success = false, Message = "Invalid Email or Password" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Exception: {ex}");
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Login error: {ex.Message}"
                };
            }
            }

        public override Task <ValidationTokenResponse> ValidationToken(ValidationTokenRequest request, ServerCallContext context)
        {
            var handler = new JwtSecurityTokenHandler ();

            try
            {
                var ValidationParametres = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
                };

                handler.ValidateToken(request.Token, ValidationParametres, out var validatedToken);
                
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                return Task.FromResult(new ValidationTokenResponse { IsValid = true, UserId = userId });
            }

            catch
            {
                return Task.FromResult(new ValidationTokenResponse { IsValid = false });
            }
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(2),
                claims: claims,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
