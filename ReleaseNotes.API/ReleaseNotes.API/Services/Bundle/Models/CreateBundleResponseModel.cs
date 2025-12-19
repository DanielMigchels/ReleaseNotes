namespace ReleaseNotes.API.Services.Bundle.Models;

public class CreateBundleResponseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}
