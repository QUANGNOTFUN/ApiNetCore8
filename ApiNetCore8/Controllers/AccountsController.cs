using ApiNetCore8.Data;
using ApiNetCore8.Models;
using ApiNetCore8.Repositores;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiNetCore8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
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
        [HttpGet("GetUser")]
        public async Task<ActionResult<ApplicationUser>> GetUserByEmployeeCode(string employeeCode)
        {
            var user = await _accountRepository.GetUserAsync(employeeCode);
            if (user == null)
            {
                return NotFound("Không tìm thấy nhân viên với mã nhân viên đã cho.");
            }

            return Ok(user);
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserByEmployeeCode(string employeeCode, UserModel model)
        {
            var result = await _accountRepository.UpdateUserAsync(employeeCode, model);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }



    }
}
