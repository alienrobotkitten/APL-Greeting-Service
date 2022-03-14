using GreetingService.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GreetingService.Infrastructure;

public class TeamsApprovalService : IApprovalService
{
    private IConfiguration _config;
    private HttpClient _client;
    private ILogger<TeamsApprovalService> _log;
    private string? IncomingWebhookUrl;
    private string ApproveUserApiEndpoint;
    private string RejectUserApiEndpoint;

    public TeamsApprovalService(ILogger<TeamsApprovalService> log, HttpClient httpClient, IConfiguration config)
    {
        _config = config;
        _client = httpClient;
        _log = log;
        IncomingWebhookUrl = config["IncomingWebhookUrl"];
        ApproveUserApiEndpoint = "https://helenatestdev.azurewebsites.net/api/user/approve/";
        RejectUserApiEndpoint = "https://helenatestdev.azurewebsites.net/api/user/reject/";

    }
    public async Task BeginUserApprovalAsync(User user)
    {
        // Please note that response body needs to be extracted and read 
        // as Connectors do not throw 429s
        string jsoncontent = 
            "{" +
            "\"@type\": \"MessageCard\"," +
            "\"@context\": \"https://schema.org/extensions\"," +
            "\"sections\": [" +
                "{" +
                    "\"title\": \"**Pending approval**\"," +
                    "\"activityImage\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/7/7c/User_font_awesome.svg/1024px-User_font_awesome.svg.png?20160212005950\","+
	    		    $"\"activityTitle\": \"Approve new user in GreetingService: {user.Email}\"," +
	    		    $"\"activitySubtitle\": \"{user.FirstName} {user.LastName}\"," +
	    		    "\"facts\": [" +"" +
                        "{" +
                            "\"name\": \"Expires on:\"," +
                            $"\"value\": \"{user.ApprovalExpiry}\"" +
                        "},"+
				        "{"+
                            "\"name\": \"Details:\"," +
					        "\"value\": \"Please approve or reject the new user for the GreetingService\""+
                        "}"+
		    	    "]"+
		        "},"+
		        "{" +
                    "\"potentialAction\": ["+
                        "{"+
                            "\"@type\": \"HttpPOST\","+
                            "\"name\": \"Approve\","+
                            $"\"target\": \"{ApproveUserApiEndpoint}{user.ApprovalCode}\""+ // approval endpoint url
                        "},"+
                        "{"+
                            "\"@type\": \"HttpPOST\","+
                            "\"name\": \"Reject\","+
                            $"\"target\": \"{RejectUserApiEndpoint}{user.ApprovalCode}\""+ // rejection endpoint url
                        "}" +
                    "]" +
                "}" +
            "]" +
        "}";
        
        //try
        //{
            // Perform Connector POST operation     
            var httpResponseMessage = await _client.PostAsync(IncomingWebhookUrl, new StringContent(jsoncontent));
            // Read response content
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            if (responseContent.Contains("Microsoft Teams endpoint returned HTTP error 429"))
            {
                // initiate retry logic
            }
        //}
        //catch (Exception ex)
        //{

        //}

    }
}
