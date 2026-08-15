namespace Shared.Redis.Common
{
    public sealed class UserSession(string sessionId, string encryptedAccessToken, string encryptedRefreshToken, DateTime accessTokenExpiresAt, string userId)
    {
        public string SessionId { get; set; } = sessionId;
        public string EncryptedAccessToken { get; set; } = encryptedAccessToken;
        public string EncryptedRefreshToken { get; set; } = encryptedRefreshToken;
        public DateTime AccessTokenExpiresAt { get; set; } = accessTokenExpiresAt;
        public string UserId { get; set; } = userId;
    }
}