using System;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly ForgeService _forgeService;

    public AuthController(ILogger<AuthController> logger, ForgeService forgeService)
    {
        _logger = logger;
        _forgeService = forgeService;
    }


    [HttpGet("login")]
    public IActionResult Login(string clientId, string clientSecret)
    {
        var redirectUri = _forgeService.GetAuthorizationURL(clientId, clientSecret);
        return Ok(redirectUri);
    }

    [HttpGet("logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("public_token");
        _forgeService.internalToken = null;
        Response.Cookies.Delete("refresh_token");
        Response.Cookies.Delete("expires_at");
        return Redirect("/");
    }

    [HttpGet("callback")]
    public async Task<ActionResult> Callback(string code)
    {
        var tokens = await _forgeService.GenerateTokens(code);
        _forgeService.internalToken = tokens.InternalToken;
        Response.Cookies.Append("public_token", tokens.PublicToken);
        Response.Cookies.Append("refresh_token", tokens.RefreshToken);
        Response.Cookies.Append("expires_at", tokens.ExpiresAt.ToString());
        return Redirect("http://localhost:8080");
    }

    [HttpGet("user")]
    public async Task<dynamic> GetProfile()
    {
        var tokens = await _forgeService.PrepareTokens(Request, Response, _forgeService);
        if (tokens == null)
        {
            return Unauthorized();
        }
        dynamic profile = await _forgeService.GetUserProfile(tokens);
        return profile;
    }

    [HttpGet("token")]
    public async Task<dynamic> GetPublicToken()
    {
        var tokens = await _forgeService.PrepareTokens(Request, Response, _forgeService);
        if (tokens == null)
        {
            return Unauthorized();
        }
        return new
        {
            access_token = tokens.PublicToken,
            token_type = "Bearer",
            expires_in = Math.Floor((tokens.ExpiresAt - DateTime.Now.ToUniversalTime()).TotalSeconds)
        };
    }
}