using Tweetinvi.Models;

namespace Examplinvi.ASP.NET.Core
{
    public static class MyCredentials
    {
        public static ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials("CQ3wb5HzDRLUi0CsQ2C2gAQZQ", "v2w1WMKTr1PKAKF2tFHbPhqCWvXzHyX0cXThmHBQAnAJCYkvai", "821546598-WrLaFzkyyK5KXPUAuGHvnm0JlbkCRpisnvwgnMoj", "cSXVuBUOMOMrOoOrGKYVgbwoS1s7JeNR9T7INH7acZ325");
        }

        public static ITwitterCredentials GenerateAppCreds()
        {
            var userCreds = GenerateCredentials();
            return new TwitterCredentials(userCreds.ConsumerKey, userCreds.ConsumerSecret);
        }
    }
}