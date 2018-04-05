// <copyright file="AcmStatisticsAbpWebCoreModule.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>


#if FEATURE_SIGNALR
using Abp.Web.SignalR;
#elif FEATURE_SIGNALR_ASPNETCORE
using Abp.AspNetCore.SignalR;
#endif

namespace AcmStatisticsAbp
{
    using System;
    using System.Text;
    using Abp.AspNetCore;
    using Abp.AspNetCore.Configuration;
    using Abp.Modules;
    using Abp.Reflection.Extensions;
    using Abp.Zero.Configuration;
    using AcmStatisticsAbp.Authentication.JwtBearer;
    using AcmStatisticsAbp.Configuration;
    using AcmStatisticsAbp.EntityFrameworkCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    [DependsOn(
         typeof(AcmStatisticsAbpApplicationModule),
         typeof(AcmStatisticsAbpEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
#if FEATURE_SIGNALR 
        ,typeof(AbpWebSignalRModule)
#elif FEATURE_SIGNALR_ASPNETCORE
        ,typeof(AbpAspNetCoreSignalRModule)
#endif
     )]
    public class AcmStatisticsAbpWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AcmStatisticsAbpWebCoreModule(IHostingEnvironment env)
        {
            this._env = env;
            this._appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            this.Configuration.DefaultNameOrConnectionString = this._appConfiguration.GetConnectionString(
                AcmStatisticsAbpConsts.ConnectionStringName);

            // Use database for language management
            this.Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            this.Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(AcmStatisticsAbpApplicationModule).GetAssembly());

            this.ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            this.IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = this.IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this._appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = this._appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = this._appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(typeof(AcmStatisticsAbpWebCoreModule).GetAssembly());
        }
    }
}
