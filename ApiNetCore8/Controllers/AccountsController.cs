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

        [HttpGet("get-all-users")]
        public async Task<ActionResult<List<UserModel>>> GetAllUsers()
        {
            try
            {
                var users = await _accountRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi trong hệ thống.", Details = ex.Message });
            }
        }


        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserByEmpyeeId(string EmployeeId)
        {
            var user = await _accountRepository.GetUserAsync(EmployeeId);
            if (user == null)
            {
                return NotFound("Không tìm thấy nhân viên với mã nhân viên đã cho.");
            }

            return Ok(user);
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserByEmpyeeId(string EmployeeId, UserModel model)
        {
            try
            {
                var result = await _accountRepository.UpdateUserAsync(EmployeeId, model);
                if (!result.Succeeded)
                {
                    return BadRequest(new { Message = "Cập nhật không thành công.", Errors = result.Errors });
                }

                return Ok(new { Message = "Thông tin người dùng đã được cập nhật thành công." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

    }
}
