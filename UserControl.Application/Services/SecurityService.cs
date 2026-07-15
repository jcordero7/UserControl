using UserControl.Core.Entities;
using UserControl.Core.Interfaces;
using System.Threading.Tasks;
using UserControl.Application.Interfaces.services;

namespace UserControl.Application.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly PaginationOptions _paginationOptions;
        //private readonly IRepository<Post> _postRepository;
        //private readonly IRepository<User> _userRepository;

        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<Security> GetLoginByCredentials(UserLogin userLogin)
        //{
        //    return await _unitOfWork.SecurityRepository.GetLoginByCredentials(userLogin);
        //}

        public async Task<Security> GetLoginByCredentials(UserLogin userLogin)
        {
            return await _unitOfWork.SecurityRepository.GetLoginByCredentials(userLogin);

            //var result = await _unitOfWork.UserRepository.GetLoginByCredentials(userLogin);

            //var security = new Security()
            //{
            //    Id = result.Id,
            //    User = result.Email,
            //    UserName = result.Names + " " + result.SurNames,
            //    Password = result.Password
                
            //};

            //return security;

        }

        public async Task RegisterUser(Security security)
        {
            await _unitOfWork.SecurityRepository.Add(security);
            await _unitOfWork.SaveChangesAsyn();
        }


    }
}
