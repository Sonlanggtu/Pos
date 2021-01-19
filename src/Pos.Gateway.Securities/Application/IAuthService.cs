using Pos.Gateway.Securities.Models;
using Pos.Gateway.Securities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Gateway.Securities.Application
{
    public interface IAuthService
    {
        Task<object> GenerateTokenJWT(AspNetUsers user);

        Task<object> CreateAsync(AspNetUserVm user);
    }
}
