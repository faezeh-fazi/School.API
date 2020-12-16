using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject
{
   public class AuthenticationResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
