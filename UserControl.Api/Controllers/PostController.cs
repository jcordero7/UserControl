using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserControl.Api.Responses;
using UserControl.Core.CustomEntities;
using UserControl.Core.DTOs;
using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using UserControl.Core.QueryFilters;
using UserControl.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UserControl.Application.Interfaces.services;
using Microsoft.AspNetCore.Http;
using UserControl.Application.Responses;

namespace UserControl.Api.Controllers
{  
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public PostController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            _postService = postService;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Retrieve all posts
        /// </summary>
        /// <param name="filters">Filters to apply</param>
        /// <returns></returns>
        [HttpGet (Name = nameof(GetPost))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponses<IEnumerable<PostDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        // public async Task<IActionResult> GetPosts()
        public IActionResult GetPosts([FromQuery] PostQueryFilter filters)
        {
            // var posts = await _postService.GetPosts();

            var posts = _postService.GetPosts(filters);
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            //var response = new ApiResponses<IEnumerable<PostDto>>(postsDto);

            var metadata = new Metadata
            {
                TotalCount = posts.TotalCount,
                PageSize = posts.PageSize,
                CurrentPage = posts.CurrentPage,
                TotalPages = posts.TotalPages,
                HasNextPage = posts.HasNextPage,
                HasPreviousPage = posts.HasPreviousPage,
               // NextPageUrl = _uriService.GetPostPaginationUri(filters, "api/Post").ToString()

                NextPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPost))).ToString(),
                PreviousPageUrl = _uriService.GetPostPaginationUri(filters, Url.RouteUrl(nameof(GetPost))).ToString()

                //   PreviousPageUrl = _uriService.GetPostPaginationUri(filters, "api/Post").ToString()
            };

            var response = new ApiResponses<IEnumerable<PostDto>>(postsDto)
            {
                Meta = metadata
            };

            Response.Headers.Append("x-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            var postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponses<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            await _postService.InsertPost(post);

            postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponses<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.Id = id;
            var result = await _postService.UpdatePost(post);
            var response = new ApiResponses<bool>(result);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.DeletePost(id);
            var response = new ApiResponses<bool>(result);
            return Ok(response);
        }

    }
}
