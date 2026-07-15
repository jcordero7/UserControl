using Microsoft.EntityFrameworkCore;
using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using UserControl.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserControl.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
      //  private readonly SocialMediaContext _context;

        public PostRepository(UserControlContext context) : base(context)
        { }

        public async Task<IEnumerable<Post>> GetPostsByUser(int UserId)
        {
            //se usa toList porque son pocos registros los que se devuelven
            return await _entities.Where(x => x.UserId == UserId).ToListAsync();
        }

        //public async Task<IEnumerable<Post>> GetPostsByUser(int UserId)
        //{
        //    //se usa toList porque son pocos registros los que se devuelven
        //    return await _entities.Where(x => x.UserId == UserId).ToListAsync();
        //}

    }

}
