using Microsoft.AspNetCore.Identity;

namespace ReleaseNotes.API.Data.Models;

public class User : IdentityUser
{
    public bool HasAccess { get; set; } = false;
    public virtual ICollection<Project> Projects { get; set; } = [];
}