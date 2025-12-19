namespace ReleaseNotes.API.Services.Seed;

public interface ISeedService
{
    public Task SeedFakeData();
    public Task SeedMassiveFakeData();
}
