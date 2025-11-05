using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoWave.Infra.Context;
using TechnoWave.Core.Models.ResponseModels;
using TechnoWave.Core.Models.RequestModels;

namespace TechnoWave.Business.Repository.Interfaces
{
    public interface IAuthService :IGenericRepository<TblUser>
    {
        Task<IResponseVm<LoginResponseVm>> UserLogin(UserLoginRequest userLoginRequest);
        Task<IResponseVm<string>> Register(RegisterUserRequest register);
    }
}
