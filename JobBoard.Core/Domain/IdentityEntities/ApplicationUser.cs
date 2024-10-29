﻿using Microsoft.AspNetCore.Identity;

namespace JobBoard.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
