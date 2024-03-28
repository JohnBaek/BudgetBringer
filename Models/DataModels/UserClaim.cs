using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

public class UserClaim : IdentityUserClaim<Guid>
{
    public virtual User User { get; set; }
}