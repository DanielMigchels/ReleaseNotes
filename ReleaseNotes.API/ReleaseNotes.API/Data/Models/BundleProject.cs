namespace ReleaseNotes.API.Data.Models;

public class BundleProject
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BundleId { get; set; }
    public virtual Bundle? Bundle { get; set; }
    public Guid ProjectId { get; set; }
    public virtual Project? Project { get; set; }
}
