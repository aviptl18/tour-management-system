using System.Security.Claims;

namespace Tour_Module.Results
{
    public class AuthenticationResult
    {
        public bool Succeeded { get; init; }

        public bool InvalidCredentials { get; init; }

        public bool Inactive { get; init; }

        public bool LockedOut { get; init; }

        public ClaimsPrincipal? Principal { get; init; }

        public int? UserId { get; init; }

        public List<string> Roles { get; init; } = new();
    }
}