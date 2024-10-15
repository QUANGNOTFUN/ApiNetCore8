using ApiNetCore8.Data;
using ApiNetCore8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiNetCore8.Repositores
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private SignInManager<ApplicationUser> SignInManager;
        private readonly IConfiguration configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration
            configuration)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.configuration  = configuration;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await SignInManager.PasswordSignInAsync
                (model.Email, model.Password, false, false);
            if (!result.Succeeded) {
                return string.Empty;
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid
                    ().ToString())
            };
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration
                ["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer : configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires : DateTime.Now.AddMinutes(20),
                claims : authClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey,
                    SecurityAlgorithms.HmacSha256Signature)
           );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };
            return await UserManager.CreateAsync(user,model.Password);
        }
    }
}
