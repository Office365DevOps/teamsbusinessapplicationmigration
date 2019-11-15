using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using static Microsoft.Teams.Samples.TaskModule.Web.MessageExtension;

namespace Microsoft.Teams.Samples.TaskModule.Web.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            using (var connector = new ConnectorClient(new Uri(activity.ServiceUrl)))
            {
                if (activity.IsComposeExtensionQuery())
                {
                    var response = MessageExtension.HandleMessageExtensionQuery(connector, activity);
                    if (response == null)
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    if (response.GetType() == typeof(ComposeExtensionResponse))
                    {
                        return Request.CreateResponse<ComposeExtensionResponse>((ComposeExtensionResponse)response);
                    }

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    var result = await Bot.HandleActivity(connector, activity);
                    return result == null ?
                    new HttpResponseMessage(HttpStatusCode.OK) :
                    Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
        }
    }
}
