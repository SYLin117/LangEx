namespace API.Entities;

public class Request
{
    public AppUser SourceUser { get; set; }
    public int SourceUserId { get; set; }
    public AppUser TargetUser { get; set; }
    public int TargetUserId { get; set; }

    public bool Accept { get; set; }
    public bool Block { get; set; }
}