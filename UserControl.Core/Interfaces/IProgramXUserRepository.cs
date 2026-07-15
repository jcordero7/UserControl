using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserControl.Core.Entities;

namespace UserControl.Core.Interfaces
{
    public interface IProgramXUserRepository : IRepository<ProgramXUser>
    {
        List<ProgramXUser> GetRoleByUserProg(int userId);
    }
}
