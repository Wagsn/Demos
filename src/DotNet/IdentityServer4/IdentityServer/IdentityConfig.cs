using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class IdentityConfig
    {
        /// <summary>
        /// 客户端
        /// </summary>
        public static IEnumerable<Client> Clients { get; set; } = new List<Client>
            {
                new Client
                {
                    ClientId = "weatherApi",
                    ClientName = "ASP.NET Core Weather Api",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    //AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    ClientSecrets = new List<Secret> {new Secret("ProCodeGuide".Sha256())},
                    AllowedScopes = new List<string> {"weatherApi.read"},
                    // 请求刷新令牌true
                    AllowOfflineAccess = true,
                    // 刷新令牌时，刷新令牌句柄将保持不变
                    //RefreshTokenUsage = TokenUsage.ReUse,
                    // 不校验 ClientSecret
                    //RequireClientSecret = false,
                },
                new Client
                {
                    ClientId = "oidcMVCApp",
                    // Consent redirect client name
                    ClientName = "Sample ASP.NET Core MVC Web App",
                    // Consent redirect uri
                    ClientUri = "https://localhost:44308",
                    ClientSecrets = new List<Secret> {new Secret("ProCodeGuide".Sha256())},

                    // https://www.cnblogs.com/pangjianxin/p/9279865.html
                    //AllowedGrantTypes = GrantTypes.Code,
                    // 将implicit和authorization code或者hybrid组合在一起会造成从更安全的基于authorization code流程模式降级到implicit流模式。
                    // 同样的问题也存在于authorization code和hybrid混合使用
                    // Grant types list cannot contain both implicit and authorization_code
                    // Grant types list cannot contain both implicit and hybrid
                    // Grant types list cannot contain both authorization_code and hybrid
                    AllowedGrantTypes = { 
                        // 授权码模式（authorization_code）
                        GrantType.AuthorizationCode, 
                        // 客户端模式（client_credentials） client_id,client_secret
                        GrantType.ClientCredentials, 
                        // 密码模式（password） 建议是使用隐式或混合的交互流来代替用户身份验证 实现IResourceOwnerPasswordValidator来提供自定义用户校验
                        GrantType.ResourceOwnerPassword, 
                        // 简化模式（implicit）
                        //GrantType.Implicit,
                        // 混合模式 implicit和authorization code的混合
                        // GrantType.Hybrid,
                    },
                    RedirectUris = new List<string> 
                    {
                        // For MvcClient
                        "https://localhost:44308/signin-oidc", 
                        // For ConsoleClient
                        "http://127.0.0.1:45656"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "weatherApi.read"
                    },
                    
                    // Consent
                    RequireConsent = true,
                    AllowRememberConsent = true,

                    // ture: code_challenge is missing
                    RequirePkce = false,
                    AllowPlainTextPkce = false,
                    // 启用对刷新令牌的支持
                    AllowOfflineAccess = true,
                    // 浏览器通道传输 token
                    //AllowAccessTokensViaBrowser = true,
                },
                // OpenID Connect implicit flow client (MVC)  采用隐式认证的MVC客户端
                new Client
                {
                    ClientId = "mvc.implicit",
                    ClientName = "MVC Implicit Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:44308/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:44308/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                // HybridAndClientCredentials  code_challenge is missing
                new Client
                {
                    ClientId = "mvc.hybrid",
                    ClientName = "MVC Hybrid Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // where to redirect to after login
                    RedirectUris = {
                        "https://localhost:44308/signin-oidc", 
                    },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:44308/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "weatherApi.read",
                    },
                    
                    // 为了请求一个刷新令牌，客户端需要在请求中包含一个offline_access的scope（并且这个scope必须通过认证）
                    AllowOfflineAccess = true,
                },
            };

        /// <summary>
        /// 标识资源
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources { get; set; } = 
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                },
            };

        /// <summary>
        /// API资源 名称将会包含在accesstoken的aud声明中
        /// aud用于webapi来验证自己定义的Audience
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources { get; set; } =
            new[]
            {
                new ApiResource
                {
                    Name = "weatherApi",
                    DisplayName = "Weather Api",
                    Description = "Allow the application to access Weather Api on your behalf",
                    Scopes = new List<string> { "weatherApi.read", "weatherApi.write"},
                    ApiSecrets = new List<Secret> {new Secret("ProCodeGuide".Sha256())},
                    UserClaims = new List<string> {"role"}
                }
            };

        /// <summary>
        /// 表示主题（用户）授予客户端的权限（以范围为单位） 第三方应用授权获取用户的客户端权限范围
        /// </summary>
        public static IEnumerable<Consent> Consents { get; set; } =
            new[]
            {
                new Consent
                {
                    SubjectId = "56892347",
                    ClientId = "oidcMVCApp",
                    Scopes = new[]
                    {
                        "weatherApi.read",
                        "weatherApi.write",
                    },
                    CreationTime = DateTime.Now,
                    Expiration = DateTime.MaxValue,
                }
            };

        /// <summary>
        /// API域 类似 Permission
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes { get; set; } = 
            new[]
            {
                new ApiScope("weatherApi.read", "Read Access to Weather API"),
                new ApiScope("weatherApi.write", "Write Access to Weather API"),
            };

        /// <summary>
        /// 测试用户
        /// </summary>
        public static List<TestUser> TestUsers { get; set; } = 
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "56892347",
                    Username = "procoder",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "support@procodeguide.com"),
                        new Claim(JwtClaimTypes.Role, "admin"),
                        new Claim(JwtClaimTypes.WebSite, "https://procodeguide.com")
                    }
                }
            };
    }
}
