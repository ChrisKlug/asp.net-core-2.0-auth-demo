using System.Threading.Tasks;

namespace Web.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<User> Add(string name, string email, string password);

        Task<User> AuthenticateExternal(string id);
        Task<User> AddExternal(string id, string name, string email);
    }
}
