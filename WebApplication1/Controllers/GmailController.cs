using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Interfaces;

[Authorize]
public class GmailController : Controller
{
    private readonly IGmailServiceHelper _gmailServiceHelper;
    private readonly IEmailParser _emailParser;
    private readonly IGmailApiWrapper _apiWrapper;

    public GmailController(IGmailServiceHelper gmailServiceHelper, IEmailParser emailParser, IGmailApiWrapper apiWrapper)
    {
        _gmailServiceHelper = gmailServiceHelper;
        _emailParser = emailParser;
        _apiWrapper = apiWrapper;
    }

    public async Task<IActionResult> Index(string keyword, string before, string after, int page = 1)
    {
        // Server validation
        if (!string.IsNullOrEmpty(keyword) && keyword.Length > 100)
            return BadRequest("Keyword is too long (max 100 chars)");

        DateTime parsedBefore, parsedAfter;
        if (!string.IsNullOrEmpty(before) && !DateTime.TryParse(before, out parsedBefore))
            return BadRequest("Invalid 'before' date format. Use YYYY-MM-DD.");
        if (!string.IsNullOrEmpty(after) && !DateTime.TryParse(after, out parsedAfter))
            return BadRequest("Invalid 'after' date format. Use YYYY-MM-DD.");
        if (!string.IsNullOrEmpty(before) && !string.IsNullOrEmpty(after) && DateTime.TryParse(before, out parsedBefore) && DateTime.TryParse(after, out parsedAfter) && parsedAfter > parsedBefore)
            return BadRequest("'After' date cannot be later than 'before' date.");
        // End of server validation

        try
        {
            const int pageSize = 10;

            var token = HttpContext.Session.GetString("AccessToken");
            if (token == null)
                return RedirectToAction("Login", "Auth");

            var service = _apiWrapper.CreateService(token);

            var query = keyword ?? "";
            if (!string.IsNullOrEmpty(before)) query += $" before:{before}";
            if (!string.IsNullOrEmpty(after)) query += $" after:{after}";

            var allMessages = await _gmailServiceHelper.SearchMessages(service, query);
            var totalCount = allMessages.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedMessages = allMessages
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var emailList = new List<EmailViewModel>();

            foreach (var msg in pagedMessages)
            {
                var message = await _gmailServiceHelper.GetMessage(service, msg.Id);
                emailList.Add(_emailParser.ParseMetadata(message));
            }

            var viewModel = new EmailPageViewModel
            {
                Emails = emailList,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while loading emails: {ex.Message}");
        }
    }

    public async Task<IActionResult> ViewMessage(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest("Message ID is required");

        var token = HttpContext.Session.GetString("AccessToken");
        if (token == null)
            return Unauthorized();

        try
        {
            var service = _apiWrapper.CreateService(token);
            var msg = await _gmailServiceHelper.GetMessage(service, id);

            var decodedHtml = _emailParser.ParseHtmlBody(msg);
            return Content(decodedHtml, "text/html");
        }
        catch (Exception ex)
        {
            return Content($"<p><strong>Error loading message:</strong> {ex.Message}</p>", "text/html");
        }
    }
}
