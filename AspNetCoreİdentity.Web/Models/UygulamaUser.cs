﻿using Microsoft.AspNetCore.Identity;

namespace AspNetCoreİdentity.Web.Models
{
    public class UygulamaUser : IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }
    }
}
