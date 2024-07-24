using StudentSupervisorService.Models.Response.UserResponse;
using StudentSupervisorService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Service
{
    public interface LoginService
    {
        Task<(bool success, string message, string token)> Login(LoginModel login, bool isAdmin);
        void Logout(string token);
    }
}
