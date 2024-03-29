using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Forge;

namespace API.Models
{
   public class Tokens
{
    public string? InternalToken;
    public string? PublicToken;
    public string? RefreshToken;
    public DateTime ExpiresAt;
}

public partial class ForgeService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _callbackUri;
    public string? internalToken { get; set; }
    private readonly Scope[] InternalTokenScopes = new Scope[] { Scope.DataRead, Scope.ViewablesRead };
    private readonly Scope[] PublicTokenScopes = new Scope[] { Scope.ViewablesRead };

    public ForgeService(string clientId, string clientSecret, string callbackUri)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _callbackUri = callbackUri;
    }
}
}