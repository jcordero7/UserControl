using Microsoft.Extensions.Options;
using UserControl.Core.CustomEntities;
using UserControl.Core.Entities;
using UserControl.Core.Exeptions;
using UserControl.Core.Interfaces;
using UserControl.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserControl.Application.Interfaces.services;

namespace UserControl.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;
        //private readonly IRepository<Post> _postRepository;
        //private readonly IRepository<User> _userRepository;

        public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }
        //search
        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {
            //return _unitOfWork.PostRepository.GetAll();

            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

           var posts = _unitOfWork.PostRepository.GetAll();

            if (filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }

            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }

            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagedPosts = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);

            return pagedPosts;
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        //
        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.PostRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new BusinessExceptions("El usuario no existe");
            }

            var userPost = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);

            if (userPost.Count() < 10)
            {
                var lastPost = userPost.OrderByDescending(x => x.Date).FirstOrDefault();
                if (lastPost != null &&  (DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessExceptions("El usuario aun no tiene permitido publicar mas de una publicación por semana");
                }
            }


            if(post.Description.Contains("Sexo"))
            {
                throw new BusinessExceptions("Contenido no permitido");
            }

            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsyn();
        }

        public async Task<bool> UpdatePost(Post post)
        {

            var existingPost = await _unitOfWork.PostRepository.GetById(post.Id);

            if (existingPost != null)
            {
                existingPost.Description = post.Description;
                existingPost.Image = post.Image;

                _unitOfWork.PostRepository.Update(existingPost);
                await _unitOfWork.SaveChangesAsyn();
                return true;
            }
            else
                return false;
        }
        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsyn();
            return true;
        }

       
    }
}
