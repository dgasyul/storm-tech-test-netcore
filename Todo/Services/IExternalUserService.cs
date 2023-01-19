using System.Threading.Tasks;

namespace Todo.Services
{
    public interface IExternalUserService
    {
        Task<string> GetNameByEmailAsync(string email);
    }
}
