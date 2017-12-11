using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Web.Services;

namespace Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IUserService, DummyUserService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/login";
                    options.AccessDeniedPath = "/auth/accessdenied";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "### INSERT FACEBOOK APP ID HERE ###";
                    options.AppSecret = "### INSERT FACEBOOK APP SECRET HERE ###";

                    options.SignInScheme = "TempCookie";
                })
                .AddTwitter(options =>
                {
                    options.ConsumerKey = "### INSERT TWITTER COMSUMER KEY HERE ###";
                    options.ConsumerSecret = "### INSERT TWITTER COMSUMER SECRET HERE ###";

                    options.SignInScheme = "TempCookie";
                })
                .AddCookie("TempCookie");
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
    }
}
