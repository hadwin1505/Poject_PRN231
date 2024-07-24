using Domain.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Authentication
{
    public interface IAuthentication
    {
        bool Verify(string hashPassword, string inputPassword);
        string Hash(string password);
        string GenerateToken(User user, string secretKey, string role);
        int? GetUserIdFromContext(HttpContext context);
    }

}
