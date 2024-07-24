using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Service.Implement
{
    public class TokenBlacklistImplement : TokenBlacklistService
    {
        private readonly HashSet<string> _blacklistedTokens = new HashSet<string>();

        public void BlacklistToken(string token)
        {
            _blacklistedTokens.Add(token);
            Console.WriteLine($"Mã Token bị liệt vào danh sách đen: {token}");
        }

        public bool IsTokenBlacklisted(string token)
        {
            bool isBlacklisted = _blacklistedTokens.Contains(token);
            if (isBlacklisted)
            {
                Console.WriteLine($"Mã Token bị đưa vào danh sách đen: {token}");
            }
            return isBlacklisted;
        }
    }
}
