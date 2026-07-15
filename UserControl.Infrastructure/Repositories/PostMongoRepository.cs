using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace UserControl.Infrastructure.Repositories
{
    public class PostMongoRepository : IPostRepository
    {
        public Task Add(Post entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePost(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPost(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = Enumerable.Range(1, 10).Select(x => new Post
            {
                Id = x,
                Description = $"Description Mongo {x}",
                Date = DateTime.Now,
                Image = $"https://misapis.com/{x}",
                UserId = x * 2
            }
           );

            await Task.Delay(10);
            return posts;
        }

        public Task<IEnumerable<Post>> GetPostsByUser(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task InsertPost(Post post)
        {
            throw new NotImplementedException();
        }

        public void Update(Post entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
