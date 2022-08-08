using Duende.IdentityServer.Test;

namespace Notes.Identity.Configuration.IS4;

public static class AppApiTestUsers
{
    public static List<TestUser> ApiUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "test@mail.ru",
                Password = "123"
            },
        };
}