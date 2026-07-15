using UserControl.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserControl.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        //Task<User> GetUser(int id);
        //Task<IEnumerable<User>> GetUsers();

        Task<User> GetLoginByCredentials(UserLogin login);

        Task<bool> UserExists(string email);

        Task<User> GetByEmail(string email);

    }
}