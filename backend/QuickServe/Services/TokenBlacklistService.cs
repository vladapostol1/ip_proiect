namespace QuickServe.Services;

using QuickServe.Interfaces;

public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly HashSet<string> _blacklistedTokens = new HashSet<string>();

    public Task<bool> IsTokenBlacklistedAsync(string token)
    {
        return Task.FromResult(_blacklistedTokens.Contains(token));
    }

    //Bans tokens so you can't relogin after sign out with the same token
    public Task BlacklistTokenAsync(string token)
    {
        _blacklistedTokens.Add(token);
        return Task.CompletedTask;
    }
}
