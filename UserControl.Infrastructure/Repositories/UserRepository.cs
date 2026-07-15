using Microsoft.EntityFrameworkCore;
using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using UserControl.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace UserControl.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(UserControlContext context) : base(context)
        {
        }

        public async Task<User> GetLoginByCredentials(UserLogin login)
        {
             return await _entities.FirstOrDefaultAsync(x => x.Email == login.User);
            
            ////var resultado = (from u in _context.User
            ////                 join pu in _context.ProgramXUser on u.equals (pu.)
            ////                 join pu in _context.ProgramXUser on kar.ArticuloID equals art.ArticuloID
            ////                 join bod in db.Bodegas on art.BodegaID equals bod.BodegaID
            ///
            //return null;
        }


        public async Task<bool> UserExists(string email)
        {
            var exist = await _entities.FirstOrDefaultAsync(x => x.Email == email);
            return exist != null ? true : false;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Email == email);
        }

    }
}
