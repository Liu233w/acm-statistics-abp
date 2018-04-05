// <copyright file="AcmStatisticsAbpLocalizationConfigurer.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Localization
{
    using Abp.Configuration.Startup;
    using Abp.Localization.Dictionaries;
    using Abp.Localization.Dictionaries.Xml;
    using Abp.Reflection.Extensions;

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
