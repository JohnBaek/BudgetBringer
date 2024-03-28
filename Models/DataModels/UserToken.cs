using Microsoft.AspNetCore.Identity;

namespace Models.DataModels;

public class UserToken : IdentityUserToken<Guid>
{
    public virtual User User { get; set; }
}
