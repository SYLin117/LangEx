namespace ErrorProject.Entities;

public class AppUserLanguage
{
    public int UserId { get; set; }
    public AppUser User { get; set; }

    public int LanguageId { get; set; }
    public Language Language { get; set; }
}