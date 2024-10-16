﻿using ApiNetCore8.Models;
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
                return Unauthorized(new { Message = "Invalid login attempt." });
            }
            return Ok(token);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _accountRepository.SignUpAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "User created successfully." });
        }
    }
}
