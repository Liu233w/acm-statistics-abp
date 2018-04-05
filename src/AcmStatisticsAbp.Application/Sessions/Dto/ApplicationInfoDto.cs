namespace AcmStatisticsAbp.Sessions.Dto
{
    using System;
    using System.Collections.Generic;

    public class ApplicationInfoDto
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Dictionary<string, bool> Features { get; set; }
    }
}
