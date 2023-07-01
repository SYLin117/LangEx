namespace API.Entities;

public class AppUserInterestedLanguage
{
    public int UserId { get; set; }
    public AppUser User { get; set; }

    public int LanguageId { get; set; }
    public Language Language { get; set; }
}