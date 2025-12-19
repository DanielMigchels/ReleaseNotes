namespace ReleaseNotes.API.Services.Bundle.Models;

public class AddProjectsToBundleRequestModel
{
    public required List<Guid> Projects { get; set; }
}
