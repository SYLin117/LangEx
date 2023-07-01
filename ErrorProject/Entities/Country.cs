namespace ErrorProject.Entities;

public class Country
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }

    public List<AppUser> Users { get; set; } = new();
}