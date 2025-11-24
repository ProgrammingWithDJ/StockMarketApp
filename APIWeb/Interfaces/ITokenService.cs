using APIWeb.Models;

namespace APIWeb.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
