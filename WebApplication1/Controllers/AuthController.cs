using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    public AuthController(IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Login()
    {
        var redirectUri = _config["GoogleAuth:RedirectUri"];
        var clientId = _config["GoogleAuth:ClientId"];
        var scope = "https://www.googleapis.com/auth/gmail.readonly";

        var url = $"https://accounts.google.com/o/oauth2/v2/auth" +
                  $"?client_id={clientId}" +
                  $"&redirect_uri={redirectUri}" +
                  $"&response_type=code" +
                  $"&scope={scope}" +
                  $"&access_type=offline";

        return Redirect(url);
    }

    public async Task<IActionResult> Callback(string code)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest("Missing authorization code");

        try
        {
            var clientId = _config["GoogleAuth:ClientId"];
            var clientSecret = _config["GoogleAuth:ClientSecret"];
            var redirectUri = _config["GoogleAuth:RedirectUri"];

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"code", code },
                {"client_id", clientId },
                {"client_secret", clientSecret },
                {"redirect_uri", redirectUri },
                {"grant_type", "authorization_code" }
            })
            };

            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(tokenRequest);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<JsonElement>(json);

            var accessToken = token.GetProperty("access_token").GetString();

            HttpContext.Session.SetString("AccessToken", accessToken);

            // Authenticate the user with a cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "GoogleUser")
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return RedirectToAction("Index", "Gmail");
        }
        catch (HttpRequestException ex)
        {
            // Request failed, API is not available or token is not issued
            return StatusCode(503, $"Token request failed: {ex.Message}");
        }
        catch (JsonException)
        {
            return BadRequest("Failed to parse token response.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }


    [Authorize]
    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
