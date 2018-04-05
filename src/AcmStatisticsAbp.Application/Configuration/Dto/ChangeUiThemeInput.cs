namespace AcmStatisticsAbp.Configuration.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeUiThemeInput
    {
        [Required]
        [StringLength(32)]
        public string Theme { get; set; }
    }
}
