using Newtonsoft.Json;

namespace FunticoSDK.Runtime.Scripts.Models
{
    /// <summary>
    /// Represents a single entry in the leaderboard.
    /// The GetLeaderboard API returns a list of these objects.
    /// </summary>
    public class LeaderboardEntry
    {
        [JsonProperty("place")]
        public int Place { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }

        [JsonProperty("user")]
        public UserData User { get; set; }
    }

    /// <summary>
    /// Represents the user data associated with a leaderboard entry.
    /// </summary>
    public class UserData
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("profile_picture_url")]
        public string ProfilePictureUrl { get; set; }

        [JsonProperty("border_url")]
        public string BorderUrl { get; set; }

        [JsonProperty("is_kyc_verified")]
        public bool IsKycVerified { get; set; }
    }
}