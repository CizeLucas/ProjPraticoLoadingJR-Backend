using Microsoft.AspNetCore.Identity;

namespace PublicationsAPI.Validations
{
    public class UserValidations<TUser> : IUserValidator<TUser> where TUser : IdentityUser<int>
    {

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();

            if(string.IsNullOrEmpty(user.UserName))
                return IdentityResult.Success;

            var existingUser = await manager.FindByNameAsync(user.UserName);
            
            if (existingUser != null && existingUser.Id != user.Id)
            {
                errors.Add(new IdentityError
                {
                    Code = "Duplicate UserName",
                    Description = $"Username '{user.UserName.ToLower()}' is already taken."
                });
            }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}