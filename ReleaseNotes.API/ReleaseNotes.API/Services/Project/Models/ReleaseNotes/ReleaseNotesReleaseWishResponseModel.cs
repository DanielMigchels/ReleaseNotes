namespace ReleaseNotes.API.Services.Project.Models.ReleaseNotes;

public class ReleaseNotesReleaseWishResponseModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid? SolvedInId { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
}
