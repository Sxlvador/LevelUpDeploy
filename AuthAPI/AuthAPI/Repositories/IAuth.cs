using AuthAPI.Models;

namespace AuthAPI.Repositories
{
    public interface IAuth
    {
        public Task<ResponseVM> RegisterUserAsync(RegisterVM registerVM);
        public Task<ResponseVM> LoginUserAsync(LoginVM loginVM);

    }
}
