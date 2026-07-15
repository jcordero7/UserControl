using UserControl.Core.Entities;
using System.Threading.Tasks;

namespace UserControl.Application.Interfaces.services
{
    public interface ISecurityService
    {
        Task<Security> GetLoginByCredentials(UserLogin userLogin);
        Task RegisterUser(Security security);
    }
}