using ApiNetCore8.Data;
using ApiNetCore8.Helpers;
using ApiNetCore8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                if (model.Role == "Staff")
                {
                    // Kiểm tra role Staff
                    if (!await roleManager.RoleExistsAsync(InventoryRole.Staff))
                    {
                        await roleManager.CreateAsync(new IdentityRole(InventoryRole.Staff));
                    }


                    await UserManager.AddToRoleAsync(user, InventoryRole.Staff);

                } else if (model.Role == "Manager") {
                    // Kiểm tra role Staff

                    if (!await roleManager.RoleExistsAsync(InventoryRole.Manager))
                    {
                        await roleManager.CreateAsync(new IdentityRole(InventoryRole.Manager));
                    }


                    await UserManager.AddToRoleAsync(user, InventoryRole.Manager);

                } else
                {
                    // Kiểm tra role Staff
                    if (!await roleManager.RoleExistsAsync(InventoryRole.Admin))

                    {
                        await roleManager.CreateAsync(new IdentityRole(InventoryRole.Admin));
                    }


                    await UserManager.AddToRoleAsync(user, InventoryRole.Admin);
                }

               
            }

            return result;
        }
        public async Task<UserModel> GetUserAsync(string employeeId)
        {
            if (Guid.TryParse(employeeId, out Guid employeeGuid))
            {
                var existUser = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == employeeGuid);
                if (existUser == null)
                {
                    throw new KeyNotFoundException("Không có user tồn tại");
                }

                return new UserModel
                {
                    FirstName = existUser.FirstName,
                    LastName = existUser.LastName,
                    PhoneNumber = existUser.PhoneNumber
                    // Role = existUser.Role (nếu cần)
                };
            }
            else
            {
                throw new ArgumentException("EmployeeId không hợp lệ.");
            }
        }


        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            // Lấy danh sách tất cả người dùng từ bảng AspNetUsers
            var users = await UserManager.Users.ToListAsync();

            if (users == null || !users.Any())
            {
                throw new KeyNotFoundException("Không có người dùng nào tồn tại.");
            }

            var userList = new List<UserModel>();

            // Lấy thông tin cho từng người dùng và ánh xạ sang UserModel
            foreach (var user in users)
            {
                // Lấy danh sách các roles của người dùng (nếu có)
                var roles = await UserManager.GetRolesAsync(user);

                // Thêm người dùng vào danh sách với các thông tin cần thiết
                userList.Add(new UserModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Role = string.Join(", ", roles) // Ghép các role thành một chuỗi
                });
            }

            return userList;
        }

        public async Task<IdentityResult> UpdateUserAsync(string employeeId, UserModel model)
        {
            // Kiểm tra và chuyển đổi employeeId thành Guid
            if (!Guid.TryParse(employeeId, out Guid userId))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Mã nhân viên không hợp lệ." });
            }

            // Tìm người dùng theo Id kiểu Guid
            var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy nhân viên với mã nhân viên đã cho." });
            }

            // Cập nhật thông tin người dùng
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

            // Cập nhật role nếu có trong model
            if (!string.IsNullOrEmpty(model.Role))
            {
                var currentRoles = await UserManager.GetRolesAsync(user);
                await UserManager.RemoveFromRolesAsync(user, currentRoles); // Xoá các roles cũ
                await UserManager.AddToRoleAsync(user, model.Role); // Thêm role mới
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            return await UserManager.UpdateAsync(user);
        }



    }
}
