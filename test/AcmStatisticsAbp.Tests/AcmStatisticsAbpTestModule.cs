// <copyright file="AcmStatisticsAbpTestModule.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Tests
{
    using System;
    using Abp.AutoMapper;
    using Abp.Configuration.Startup;
    using Abp.Dependency;
    using Abp.Modules;
    using Abp.Net.Mail;
    using Abp.TestBase;
    using Abp.Zero.Configuration;
    using Abp.Zero.EntityFrameworkCore;
    using AcmStatisticsAbp.EntityFrameworkCore;
    using AcmStatisticsAbp.Tests.DependencyInjection;
    using Castle.MicroKernel.Registration;
    using NSubstitute;

    [DependsOn(
        typeof(AcmStatisticsAbpApplicationModule),
        typeof(AcmStatisticsAbpEntityFrameworkModule),
        typeof(AbpTestBaseModule))]
    public class AcmStatisticsAbpTestModule : AbpModule
    {
        public AcmStatisticsAbpTestModule(AcmStatisticsAbpEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            this.Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            this.Configuration.UnitOfWork.IsTransactional = false;

            // Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            this.Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            this.Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            // Use database for language management
            this.Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            this.RegisterFakeService<AbpZeroDbMigrator<AcmStatisticsAbpDbContext>>();

            this.Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);

            // 暂时解决单元测试随机报错的问题，在 https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2735 修复之后会移除此项目
            AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue9825", true);
        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(this.IocManager);
        }

        private void RegisterFakeService<TService>()
            where TService : class
        {
            this.IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton());
        }
    }
}
