// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using IdentityServer4.Test;
using System.Collections.Generic;
using IdentityModel;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Quickstart.UI;

namespace IdentityServer
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var builder = services.AddIdentityServer(options =>
            {
                options.IssuerUri = "MyIdentityServer";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddTestUsers(new List<TestUser> {
                    new TestUser{SubjectId = "1", Username = "zerokoll", Password = "test",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Chris Klug"),
                            new Claim(JwtClaimTypes.GivenName, "Chris"),
                            new Claim(JwtClaimTypes.FamilyName, "Klug"),
                            new Claim(JwtClaimTypes.Email, "chris@59north.com"),
                        }
                    }
                });

            // in-memory, json config
            builder.AddInMemoryIdentityResources(Configuration.GetSection("IdentityResources"));
            builder.AddInMemoryApiResources(Configuration.GetSection("ApiResources"));
            builder.AddInMemoryClients(Configuration.GetSection("clients"));

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            AccountOptions.AllowRememberLogin = false;
            AccountOptions.AutomaticRedirectAfterSignOut = true;
            AccountOptions.ShowLogoutPrompt = false;
            AccountOptions.WindowsAuthenticationEnabled = false;
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}