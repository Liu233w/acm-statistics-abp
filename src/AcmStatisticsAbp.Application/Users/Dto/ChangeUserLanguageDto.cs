using System.ComponentModel.DataAnnotations;

namespace AcmStatisticsAbp.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}