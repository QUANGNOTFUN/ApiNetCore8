using ApiNetCore8.Data;
using ApiNetCore8.Helpers;
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
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration
            configuration, RoleManager<IdentityRole> roleManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.configuration  = configuration;
            this.roleManager = roleManager;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            var passwordValid = await UserManager.CheckPasswordAsync(user, model.Password);
            
            if (user == null || !passwordValid)
            {
                return string.Empty;
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            
            if (!result.Succeeded) {
                return string.Empty;
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid
                    ().ToString())
            };

            var userRoles = await UserManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }    

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer : configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires : DateTime.Now.AddMinutes(20),
                claims : authClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
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
            var result = await UserManager.CreateAsync(user,model.Password);

            if (result.Succeeded) 
            { 
                // Kiểm tra role Staff
                if(!await roleManager.RoleExistsAsync(InventoryRole.Staff))
                {
                    await roleManager.CreateAsync(new IdentityRole(InventoryRole.Staff));
                }

                await UserManager.AddToRoleAsync(user, InventoryRole.Staff);
            }

            return result;
        }
    }
}
