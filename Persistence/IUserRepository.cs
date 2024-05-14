
using Model;

namespace Persistence
{

    public interface IUserRepository : IRepository<long, User>
    {
        User FindByUsername(string username);
    }
}