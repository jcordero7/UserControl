using UserControl.Core.Entities;
using System.Threading.Tasks;

namespace UserControl.Core.Interfaces
{
    public interface ISecurityRepository : IRepository<Security>
    {
        Task<Security> GetLoginByCredentials(UserLogin login);
    }
}