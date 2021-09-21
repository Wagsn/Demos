using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.WebApi.Oauth2Client.Services
{
    public interface IIdentityServer4Service
    {
        Task<TokenResponse> GetToken(string apiScope);
    }
}
