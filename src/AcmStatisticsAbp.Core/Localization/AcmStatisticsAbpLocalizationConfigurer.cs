using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace AcmStatisticsAbp.Localization
{
    public static class AcmStatisticsAbpLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(AcmStatisticsAbpConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(AcmStatisticsAbpLocalizationConfigurer).GetAssembly(),
                        "AcmStatisticsAbp.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
