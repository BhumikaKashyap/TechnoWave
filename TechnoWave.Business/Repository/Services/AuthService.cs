using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoWave.Business.Repository.Interfaces;
using TechnoWave.Core.Common;
using TechnoWave.Core.Models.RequestModels;
using TechnoWave.Core.Models.ResponseModels;
using TechnoWave.Infra.Context;

namespace TechnoWave.Business.Repository.Services
{
    public class AuthService : BaseRepository<TechnoWaveDbContext, TblUser>, IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IResponseVm<string>> Register(RegisterUserRequest register)
        {
           IResponseVm<string> responseVm = new ResponseVm<string>();


            var userdetails = new TblUser
            {
                UserId= Guid.NewGuid(),
                Name = register.Name,
                Email = register.Email,
                PasswordHash = PasswordHelper.HashPassword(register.Password),
            };
            await Add(userdetails);

            responseVm.IsSuccess = true;
            responseVm.Message = "User Registered";
            return responseVm;
        }

        public async Task<IResponseVm<LoginResponseVm>> UserLogin(UserLoginRequest userLoginRequest)
        {
            IResponseVm<LoginResponseVm> responseVm = new ResponseVm<LoginResponseVm>();

            var userdetails = GetAllQuerable().Where(x=>x.Email==userLoginRequest.Email).FirstOrDefault();
            if (userdetails==null)
            {
                responseVm.IsSuccess = false;
                responseVm.Message = "User Not Found";
                return responseVm;
            }

            var valid = PasswordHelper.VerifyPassword(userdetails.PasswordHash, userLoginRequest.Password);

            if(!valid)
            {
                responseVm.IsSuccess = false;
                responseVm.Message = "User Not Found";
                return responseVm;
            }
            var token = JwtTokenHelper.GenerateToken(userdetails.UserId,userdetails.Email,userdetails.Name,_config);

            // generate token and save this info in claims
            LoginResponseVm loginResponseVm = new LoginResponseVm();
            loginResponseVm.Email = userdetails.Email;
            loginResponseVm.UserId = userdetails.UserId;
            loginResponseVm.Name = userdetails.Name;
            loginResponseVm.Token = token;
            
            responseVm.IsSuccess=true;
            responseVm.Message = "Login Successfully";
            responseVm.Data = loginResponseVm;
            return responseVm;


        }
    }
}
