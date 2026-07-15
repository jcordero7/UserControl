using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserControl.Application.Responses;
using UserControl.Core.CustomEntities;
using UserControl.Core.Entities;

namespace UserControl.Application.Interfaces.services
{
   public interface IUserService
    {

        Task<ApiResponses<User>> GetById(int id);

        Task<ResponseLogin> GetLoginByCredentials(UserLogin userLogin);

        Task<ResponseLogin> RegisterUser(User user);

        Task<bool> EditUser(User user);

        Task<bool> RecoverPassword(string email);

        Task<bool> NewPassword(UserNewPassword userNewPassword);

        Task<bool> ChangePassword(UserChangePassword userChangePassword);

        Task<bool> EnableAccount(UserEnableAccount userEnableAccount);

        Task<bool> ConfirmAccount(UserEnableAccount userEnableAccount);

        Task<bool> RequestToken(string email);

        
    }
}
