using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiNetCore8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IAccountRepository accountRepository)
        {
            _userManager = userManager;
            _accountRepository = accountRepository;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var token = await _accountRepository.SignInAsync(model);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "Đăng nhập không hợp lệ." });
            }
            return Ok(token);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _accountRepository.SignUpAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Đăng ký không thành công.", Errors = result.Errors });
            }
            return Ok(new { Message = "Tài khoản đã được tạo thành công." });
        }

        [HttpGet("get-info")]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var user = await _accountRepository.GetLoggedInUserAsync(User);

            if (user == null)
            {
                return Unauthorized(new { Message = "Người dùng chưa đăng nhập hoặc không tồn tại." });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                Roles = roles
            });
        }

    }
}
