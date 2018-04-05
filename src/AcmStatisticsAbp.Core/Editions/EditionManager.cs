namespace AcmStatisticsAbp.Editions
{
    using Abp.Application.Editions;
    using Abp.Application.Features;
    using Abp.Domain.Repositories;

    public class EditionManager : AbpEditionManager
    {
        public const string DefaultEditionName = "Standard";

        public EditionManager(
            IRepository<Edition> editionRepository, 
            IAbpZeroFeatureValueStore featureValueStore)
            : base(
                editionRepository,
                featureValueStore)
        {
        }
    }
}
