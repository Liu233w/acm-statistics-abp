// <copyright file="DefaultEditionCreator.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.EntityFrameworkCore.Seed.Host
{
    using System.Linq;
    using Abp.Application.Editions;
    using Abp.Application.Features;
    using AcmStatisticsAbp.Editions;
    using Microsoft.EntityFrameworkCore;

    public class DefaultEditionCreator
    {
        private readonly AcmStatisticsAbpDbContext _context;

        public DefaultEditionCreator(AcmStatisticsAbpDbContext context)
        {
            this._context = context;
        }

        public void Create()
        {
            this.CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = this._context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new Edition { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
                this._context.Editions.Add(defaultEdition);
                this._context.SaveChanges();

                /* Add desired features to the standard edition, if wanted... */
            }
        }

        private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
        {
            if (this._context.EditionFeatureSettings.IgnoreQueryFilters().Any(ef => ef.EditionId == editionId && ef.Name == featureName))
            {
                return;
            }

            this._context.EditionFeatureSettings.Add(new EditionFeatureSetting
            {
                Name = featureName,
                Value = isEnabled.ToString(),
                EditionId = editionId
            });
            this._context.SaveChanges();
        }
    }
}
