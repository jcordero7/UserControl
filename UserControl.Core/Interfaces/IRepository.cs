using UserControl.Core.Entities;
using UserControl.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserControl.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        Task<T> GetById(int Id);
        Task Add(T entity);
        void Update(T entity);
        Task Delete(int id);
    }
}
