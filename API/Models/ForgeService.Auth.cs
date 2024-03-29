using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Forge;

namespace API.Models
{
    public partial class ForgeService
{
    public string GetAuthorizationURL(string clientId, string clientSecret)
    {
        new ForgeService(clientId, clientSecret, _callbackUri);
        
        var url = new ThreeLeggedApi().Authorize(_clientId, "code", _callbackUri, InternalTokenScopes);
        return url;
    }

    public async Task<Tokens> PrepareTokens(HttpRequest request, HttpResponse response, ForgeService forgeService)
    {
        var tokens = new Tokens
        {
            PublicToken = request.Cookies["public_token"],
            InternalToken = forgeService.internalToken,
            RefreshToken = request.Cookies["refresh_token"],
            ExpiresAt = DateTime.Parse(request.Cookies["expires_at"])
        };
        if (tokens.ExpiresAt < DateTime.Now.ToUniversalTime() || tokens.InternalToken == null)
        {
            tokens = await forgeService.RefreshTokens(tokens);
            response.Cookies.Append("public_token", tokens.PublicToken);
            response.Cookies.Append("refresh_token", tokens.RefreshToken);
            response.Cookies.Append("expires_at", tokens.ExpiresAt.ToString());
        }
        return tokens;
    }

        public async Task<Tokens> GenerateTokens(string code)
    {
        dynamic internalAuth = await new ThreeLeggedApi().GettokenAsync(_clientId, _clientSecret, "authorization_code", code, _callbackUri);
        dynamic publicAuth = await new ThreeLeggedApi().RefreshtokenAsync(_clientId, _clientSecret, "refresh_token", internalAuth.refresh_token, PublicTokenScopes);
        return new Tokens
        {
            PublicToken = publicAuth.access_token,
            InternalToken = internalAuth.access_token,
            RefreshToken = publicAuth.refresh_token,
            ExpiresAt = DateTime.Now.ToUniversalTime().AddSeconds(internalAuth.expires_in)
        };
    }

    public async Task<Tokens> RefreshTokens(Tokens tokens)
    {
        dynamic internalAuth = await new ThreeLeggedApi().RefreshtokenAsync(_clientId, _clientSecret, "refresh_token", tokens.RefreshToken, InternalTokenScopes);
        dynamic publicAuth = await new ThreeLeggedApi().RefreshtokenAsync(_clientId, _clientSecret, "refresh_token", internalAuth.refresh_token, PublicTokenScopes);
        return new Tokens
        {
            PublicToken = publicAuth.access_token,
            InternalToken = internalAuth.access_token,
            RefreshToken = publicAuth.refresh_token,
            ExpiresAt = DateTime.Now.ToUniversalTime().AddSeconds(internalAuth.expires_in)
        };
    }

    public async Task<dynamic> GetUserProfile(Tokens tokens)
    {
        var api = new UserProfileApi();
        api.Configuration.AccessToken = tokens.InternalToken;
        dynamic profile = await api.GetUserProfileAsync();
        return profile;
    }
}
}