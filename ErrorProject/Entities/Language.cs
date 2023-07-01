namespace ErrorProject.Entities;

public class Language
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Code { get; set; }

    public List<AppUser> Users { get; set; }
}