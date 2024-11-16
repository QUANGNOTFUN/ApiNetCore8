using ApiNetCore8.Data;
using ApiNetCore8.Models;
using Microsoft.AspNetCore.Identity;

namespace ApiNetCore8.Repositores
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
        public Task<ApplicationUser> GetUserAsync(string employeeCode);
        public Task<IdentityResult> UpdateUserAsync(string employeeCode, UserModel model);
    }
}
