﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProductTracking.Authentication
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthServer";
        public const string AUDIENCE = "AuthClient";
        const string KEY = "qawsedrftgyhujikolp";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}