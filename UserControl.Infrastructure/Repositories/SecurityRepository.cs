using Microsoft.EntityFrameworkCore;
using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using UserControl.Infrastructure.Data;
using System.Threading.Tasks;

namespace UserControl.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(UserControlContext context) : base(context)
        {
        }

        public async Task<Security> GetLoginByCredentials(UserLogin login)
        {
            // return await _entities.FirstOrDefaultAsync(x => x.User == login.User && x.Password == login.Password);

            return await _entities.FirstOrDefaultAsync(x => x.User == login.User);
        }


    }
}
