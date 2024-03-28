using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User User { get; set; }
}