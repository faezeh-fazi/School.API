using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject
{
   public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
