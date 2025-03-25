using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;

namespace todo_app_backend.Helpers
{
    public class OAuth
    {
        public static async Task<UserCredential> GetCredentialAsync() {
            string[] scopes = { "https://mail.google.com/" };

            using var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read);
            string credPath = "token.json";

            // remove token.json/Google.Apis.Auth.OAuth2.Responses.TokenResponse-user data to clear tokens cache

            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)
            );
        }
    }
}