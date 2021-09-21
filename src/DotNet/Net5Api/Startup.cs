using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Net5Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Net5Api", Version = "v1" });
            });

            //services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
            services.AddAuthentication
                (authoption => {
                    //ָ��Ĭ��ѡ��
                    authoption.DefaultChallengeScheme = BasicAuthenticationDefaults.AuthenticationScheme;
                    authoption.DefaultAuthenticateScheme = BasicAuthenticationDefaults.AuthenticationScheme;
                    authoption.DefaultSignOutScheme = BasicAuthenticationDefaults.AuthenticationScheme;
                    authoption.DefaultSignInScheme = BasicAuthenticationDefaults.AuthenticationScheme;
                })
                //.AddCookie()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
                {
                    option.Cookie.Name = "adCookie";//���ô洢�û���¼��Ϣ���û�Token��Ϣ����Cookie����
                    option.Cookie.HttpOnly = true;//���ô洢�û���¼��Ϣ���û�Token��Ϣ����Cookie���޷�ͨ���ͻ���������ű�(��JavaScript��)���ʵ�
                    option.ExpireTimeSpan = TimeSpan.FromDays(3);// ����ʱ��
                    option.SlidingExpiration = true;// �Ƿ��ڹ���ʱ������ʱ���Զ�����
                    option.LoginPath = "/Account/Login";
                    option.LogoutPath = "/Account/LoginOut";
                })
                .AddJwtBearer(option =>
                {
                    // https://cloud.tencent.com/developer/article/1677493
                    var jwtConfig = Configuration.GetSection("Jwt").Get<JwtConfig>();
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = jwtConfig.ValidateLifetime,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SigningKey)),
                        // �������ʱ�䣬�ܵ���Чʱ��������ʱ�����jwt�Ĺ���ʱ��
                        ClockSkew = TimeSpan.FromSeconds(0)
                    };
                })
            #region Basic Auth
                .AddScheme<BasicAuthenticationOption, BasicAuthenticationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.UserName = "admin";
                    options.UserPwd = "123";
                });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Net5Api v1"));
            }

            app.UseRouting();
            app.UseCors();

            // ʹ��[Authorize]������֤
            app.UseAuthentication();
            #region Basic Auth
            // �м����ʽ��֤ ����Path��֤
            /*app.UseWhen(
                predicate: x => x.Request.Path.StartsWithSegments(new PathString("/WeatherForecast")),
                configuration: appBuilder => { appBuilder.UseBasicAuthentication(); });*/
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    #region Basic Auth

    public static class BasicAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Basic";
    }

    /// <summary>
    /// AuthenticationSchemeOptions ĳ�����������֤������ѡ��
    /// ��֤����ѡ�� ���ﲻ�ʺϴ洢�û���Ϣ
    /// </summary>
    public class BasicAuthenticationOption : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
    }

    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOption>
    {
        private readonly IOptionsMonitor<BasicAuthenticationOption> _options;
        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOption> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options;
        }

        /// <summary>
        /// ��֤
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");
            string username, password;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                username = credentials[0];
                password = credentials[1];
                var isValidUser = IsAuthorized(username, password);  // UserService.Authenticate(username, password);
                if (isValidUser == false)
                {
                    return AuthenticateResult.Fail("Invalid username or password");
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, username),  // userId.ToString()
                new Claim(ClaimTypes.Name, username),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }

        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.Realm}\"";
            await base.HandleChallengeAsync(properties);
        }

        /// <summary>
        /// ��֤ʧ��
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            await base.HandleForbiddenAsync(properties);
        }

        private bool IsAuthorized(string username, string password)
        {
            // �޸�Ϊ�����ݿ��л�ȡ�û���Ϣ UserService.Authenticate(username, password);
            return true;
        }
    }

    // HTTP������֤Middleware
    public static class BasicAuthentication
    {
        public static void UseBasicAuthentication(this IApplicationBuilder app)
        {
            app.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }

    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public BasicAuthenticationMiddleware(RequestDelegate next, ILoggerFactory LoggerFactory)
        {
            _next = next;
            _logger = LoggerFactory.CreateLogger<BasicAuthenticationMiddleware>();
        }
        public async Task Invoke(HttpContext httpContext, IAuthenticationService authenticationService)
        {
            var authenticated = await authenticationService.AuthenticateAsync(httpContext, BasicAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("Access Status��" + authenticated.Succeeded);
            if (!authenticated.Succeeded)
            {
                // ��֤ʧ�� ����ʹ��Basic Auth��֤
                await authenticationService.ChallengeAsync(httpContext, BasicAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties { });
                return;
            }
            await _next(httpContext);
        }
    }
    #endregion
}
