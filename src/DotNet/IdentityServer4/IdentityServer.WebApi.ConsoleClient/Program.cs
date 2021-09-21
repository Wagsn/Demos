using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Server;
using System.Net;
using IdentityModel.Client;
using IdentityModel.OidcClient.Browser;
using System.Net.Sockets;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.WebApi.ConsoleClient
{
    public class Program
    {
        public static void Main2(string[] args)
        {
            var browser = new SystemBrowser(45656);
            string redirectUri = "http://127.0.0.1:45656";

            var options = new OidcClientOptions
            {
                Authority = "https://localhost:44370",
                ClientId = "oidcMVCApp",
                ClientSecret = "ProCodeGuide",
                RedirectUri = redirectUri,
                Scope = "weatherApi.read openid profile",
                FilterClaims = false,
                Browser = browser,
                //IdentityTokenValidator = new JwtHandlerIdentityTokenValidator(),
                //RefreshTokenInnerHttpHandler = new HttpClientHandler()
                //Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                //ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                LoadProfile = true
            };

            var _oidcClient = new OidcClient(options);
            var result = _oidcClient.LoginAsync(new LoginRequest()).Result;
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result.AccessToken));
        }
    }
    public class SystemBrowser : IBrowser
    {
        public int Port { get; }
        private readonly string _path;

        public SystemBrowser(int? port = null, string path = null)
        {
            _path = path;

            if (!port.HasValue)
            {
                Port = GetRandomUnusedPort();
            }
            else
            {
                Port = port.Value;
            }
        }

        private int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken)
        {
            using (var listener = new LoopbackHttpListener(Port, _path))
            {
                OpenBrowser(options.StartUrl);

                try
                {
                    var result = await listener.WaitForCallbackAsync();
                    if (String.IsNullOrWhiteSpace(result))
                    {
                        return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = "Empty response." };
                    }

                    return new BrowserResult { Response = result, ResultType = BrowserResultType.Success };
                }
                catch (TaskCanceledException ex)
                {
                    return new BrowserResult { ResultType = BrowserResultType.Timeout, Error = ex.Message };
                }
                catch (Exception ex)
                {
                    return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = ex.Message };
                }
            }
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public class LoopbackHttpListener : IDisposable
    {
        const int DefaultTimeout = 60 * 5; // 5 mins (in seconds)

        IWebHost _host;
        TaskCompletionSource<string> _source = new TaskCompletionSource<string>();
        string _url;

        public string Url => _url;

        public LoopbackHttpListener(int port, string path = null)
        {
            path = path ?? String.Empty;
            if (path.StartsWith("/")) path = path.Substring(1);

            _url = $"http://127.0.0.1:{port}/{path}";

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(_url)
                .Configure(Configure)
                .Build();
            _host.Start();
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Task.Delay(500);
                _host.Dispose();
            });
        }

        void Configure(IApplicationBuilder app)
        {
            app.Run(async ctx =>
            {
                if (ctx.Request.Method == "GET")
                {
                    SetResult(ctx.Request.QueryString.Value, ctx);
                }
                else
                {
                    ctx.Response.StatusCode = 405;
                }
                await Task.CompletedTask;
            });
        }

        private void SetResult(string value, HttpContext ctx)
        {
            _source.TrySetResult(value);

            try
            {
                ctx.Response.StatusCode = 200;
                ctx.Response.ContentType = "text/html";
                ctx.Response.WriteAsync("<h1>You can now return to the application.</h1>");
                ctx.Response.Body.Flush();
            }
            catch
            {
                ctx.Response.StatusCode = 400;
                ctx.Response.ContentType = "text/html";
                ctx.Response.WriteAsync("<h1>Invalid request.</h1>");
                ctx.Response.Body.Flush();
            }
        }

        public Task<string> WaitForCallbackAsync(int timeoutInSeconds = DefaultTimeout)
        {
            Task.Run(async () =>
            {
                await Task.Delay(timeoutInSeconds * 1000);
                _source.TrySetCanceled();
            });

            return _source.Task;
        }
    }
}

namespace Client
{
    public class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("TestOAuth2");
            Console.WriteLine("TestOidc");
            Console.WriteLine("TestBrowser");
            Console.WriteLine("TestCustomBrowser");

            Console.ReadKey();
            await Task.CompletedTask;
        }

        private static async Task TestOAuth2()
        {
            // discover endpoints from metadata
            var client = new System.Net.Http.HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",

                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var apiClient = new System.Net.Http.HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:6001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = System.Text.Json.JsonSerializer.Deserialize<dynamic>(content);
                Console.WriteLine(data);
            }
        }

        private static async Task TestOidc()
        {
            var c = new IdentityModel.OidcClient.OidcClient(new IdentityModel.OidcClient.OidcClientOptions
            {

            });
            var r = await c.LoginAsync(new IdentityModel.OidcClient.LoginRequest
            {

            });

            // call api with access_token

            await Task.CompletedTask;
        }

        private static async Task TestBrowser()
        {
            ConsoleClient.Program.Main2(null);

            await Task.CompletedTask;
        }

        private static async Task TestCustomBrowser()
        {
            IdentityServer.WebApi.ConsoleClient.Program.Main2(null);

            await Task.CompletedTask;
        }

    }
}

namespace ConsoleClient
{
    public class Program
    {
        static string _authority = "https://localhost:44370";
        static string _api = "https://localhost:44370/identity";

        public static void Main2(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            Console.WriteLine("+-----------------------+");
            Console.WriteLine("|  Sign in with OIDC    |");
            Console.WriteLine("+-----------------------+");
            Console.WriteLine("");
            Console.WriteLine("Press any key to sign in...");
            Console.ReadKey();

            try
            {
                await SignInAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Catch Exception: {ex.Message}\n{ex.StackTrace}");
            }

            Console.ReadKey();
        }

        private async static Task SignInAsync()
        {
            // create a redirect URI using an available port on the loopback address.
            string redirectUri = string.Format("http://127.0.0.1:45656");

            // create an HttpListener to listen for requests on that redirect URI.
            var settings = new WebListenerSettings();
            settings.UrlPrefixes.Add(redirectUri);
            var http = new WebListener(settings);

            http.Start();
            Console.WriteLine("Listening..");

            var options = new OidcClientOptions
            {
                Authority = _authority,
                ClientId = "oidcMVCApp",
                ClientSecret = "ProCodeGuide",
                RedirectUri = redirectUri,
                Scope = "weatherApi.read openid profile",
                FilterClaims = true,
                LoadProfile = true,
            };

            var client = new OidcClient(options);
            var state = await client.PrepareLoginAsync();
            Console.WriteLine($"State: {System.Text.Json.JsonSerializer.Serialize(state)}");

            OpenBrowser(state.StartUrl);

            var context = await http.AcceptAsync();
            var formData = GetRequestPostData(context.Request);

            if (formData == null)
            {
                Console.WriteLine("Invalid response");
                return;
            }

            await SendResponseAsync(context.Response);

            var result = await client.ProcessResponseAsync(context.Request.RawUrl, state);

            ShowResult(result);
        }

        private static void ShowResult(LoginResult result)
        {
            if (result.IsError)
            {
                Console.WriteLine("\n\nError:\n{0}", result.Error);
                return;
            }

            Console.WriteLine("\n\nClaims:");
            foreach (var claim in result.User.Claims)
            {
                Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
            }

            Console.WriteLine($"\nidentity token: {result.IdentityToken}");
            Console.WriteLine($"access token:   {result.AccessToken}");
            Console.WriteLine($"refresh token:  {result?.RefreshToken ?? "none"}\n");

            var values = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(result.TokenResponse.Raw);

            Console.WriteLine($"\nRaw TokenResponse ...");
            foreach (var item in values)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        private static async Task SendResponseAsync(Response response)
        {
            string responseString = $"<html><head></head><body>Please return to the app.</body></html>";
            var buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength = buffer.Length;

            var responseOutput = response.Body;
            await responseOutput.WriteAsync(buffer, 0, buffer.Length);
            responseOutput.Flush();
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        public static string GetRequestPostData(Request request)
        {
            // AuthenticationBuilder.AddOpenIdConnect options.ResponseMode = "query";
            if (request.QueryString.StartsWith("?"))
            {
                return WebUtility.UrlDecode(request.QueryString).TrimStart('?').Replace("&", "\n").Replace("=", ":");
            }
            if (!request.HasEntityBody)
            {
                return null;
            }

            using (var body = request.Body)
            {
                using (var reader = new StreamReader(body))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}