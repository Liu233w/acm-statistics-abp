// <copyright file="InitialHostDbBuilder.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly AcmStatisticsAbpDbContext _context;

        public InitialHostDbBuilder(AcmStatisticsAbpDbContext context)
        {
            this._context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(this._context).Create();
            new DefaultLanguagesCreator(this._context).Create();
            new HostRoleAndUserCreator(this._context).Create();
            new DefaultSettingsCreator(this._context).Create();

            this._context.SaveChanges();
        }
    }
}
