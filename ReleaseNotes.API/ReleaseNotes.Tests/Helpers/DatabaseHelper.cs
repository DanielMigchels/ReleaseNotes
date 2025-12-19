using ReleaseNotes.API.Data;
using Microsoft.EntityFrameworkCore;

namespace ReleaseNotes.Tests.Helpers;

public class DatabaseHelper
{
    public static DatabaseContext CreateInMemoryDatabase()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
           .UseInMemoryDatabase(databaseName: $"InMemoryDb-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}")
           .Options;

        return new DatabaseContext(options);
    }
}
