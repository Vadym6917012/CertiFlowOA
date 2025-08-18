using System.Text.Json.Serialization;

namespace Domain.Entities.GoogleOAuth
{
    public class GoogleUserInfo
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }

        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }
        [JsonPropertyName("picture")]
        public string Picture { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("email_verified")]
        public bool emailVerified { get; set; }
    }
}
