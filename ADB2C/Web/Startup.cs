using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication(options => {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "B2C_1_sign_in_or_up";
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddOpenIdConnect("B2C_1_sign_in_or_up", options => ConfigureOpenIdOptions(options, "B2C_1_sign_in_or_up"))
                .AddOpenIdConnect("B2C_1_sign_in", options => ConfigureOpenIdOptions(options, "B2C_1_sign_in"))
                .AddOpenIdConnect("B2C_1_sign_up", options => ConfigureOpenIdOptions(options, "B2C_1_sign_up"));
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }

        private void ConfigureOpenIdOptions(OpenIdConnectOptions options, string policy)
        {
            options.MetadataAddress = "https://login.microsoftonline.com/zerokollauthdemo.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=" + policy;
            options.ClientId = "### INSERT YOUR CLIENTID HERE ###";
            options.ResponseType = OpenIdConnectResponseType.IdToken;
            options.SignedOutRedirectUri = "/";
            options.CallbackPath = "/signin-oidc-" + policy;
            options.SignedOutCallbackPath = "/signout-oidc-" + policy;
            options.TokenValidationParameters.NameClaimType = "name";
        }
    }
}
