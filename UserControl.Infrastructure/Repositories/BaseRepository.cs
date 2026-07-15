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
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly UserControlContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(UserControlContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }


        //public async Task<IEnumerable<T>> GetAll()
        public IEnumerable<T> GetAll()
        {
            // return _entities.ToListAsync();

            //ver porque se usa enumerable q list
            // var vamos = _entities.ToList();

            return _entities.AsEnumerable();
        }

        public async Task<T> GetById(int Id)
        {
            return await _entities.FindAsync(Id);
        }


        public async Task Add(T entity)
        {
           // _entities.Add(entity);
           await _entities.AddAsync(entity);

            // await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            //actualiza todos los campos
            _entities.Update(entity);
           // await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
           // _context.SaveChanges();
        }

    }
}
