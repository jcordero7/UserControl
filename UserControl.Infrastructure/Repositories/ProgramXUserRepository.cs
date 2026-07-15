using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using UserControl.Infrastructure.Data;
using System.Linq;

namespace UserControl.Infrastructure.Repositories
{
    public class ProgramXUserRepository : BaseRepository<ProgramXUser>, IProgramXUserRepository
    {
        public ProgramXUserRepository(UserControlContext context) : base(context)
        { }

        public List<ProgramXUser> GetRoleByUserProg(int userId)
        {
            //  return await _entities.FirstOrDefaultAsync(x => x.UserId == userId && x.ProgramId == programId);

            // var result = ()

            //return await _entities.FirstOrDefaultAsync(x => x.UserId == userId && x.ProgramId == programId);


            //var result = (from e in _entities.)


            // return _entities.Where(x => x.UserId == userId).ToList();

            //return _entities.Include(x => x.roleXProgram.ThenInclude(rx => rx.role)).Where(x => x.UserId == userId).ToList();

            //var result =  _entities
            //.Where(x => x.UserId == userId)
            //.Include(p => p.roleXProgram) // <-- Retorna un IIncludableQueryable
            //    .ThenInclude(rp => rp.role) // <-- Ahora sí se puede llamar ThenInclude
            //.ToListAsync();


            //el que venimos probando
           // var result = _entities
           //.Where(x => x.UserId == userId)
           //.Include(p => p.roleXProgram) // <-- Retorna un IIncludableQueryable
           //    .ThenInclude(rp => rp.role) // <-- Ahora sí se puede llamar ThenInclude
           //.Select(x => new ProgramXUser
           //{ 
           //  UserId = x.UserId,
           //  ProgramId = x.ProgramId,
           //  RoleId = x.RoleId,
           //  RoleName = x.RoleName
           //})
           //    .ToList();




            var result = _entities
            .Where(x => x.UserId == userId)
            .Select(x => new ProgramXUser
            {
                UserId = x.UserId,
                ProgramId = x.ProgramId,
                RoleId = x.RoleId,

                // EF Core verá esta navegación y generará el JOIN SQL automáticamente:
                RoleName = x.roleXProgram.role.Name
            })
            .ToList();


                    return result;

        }
    }

}
