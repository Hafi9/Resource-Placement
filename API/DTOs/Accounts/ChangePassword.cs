﻿namespace API.DTOs.Accounts
{
    public class ChangePassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public int OTP { get; set; }
    }
}
