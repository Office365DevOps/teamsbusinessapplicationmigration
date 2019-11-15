using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using Newtonsoft.Json;

namespace Microsoft.Teams.Samples.TaskModule.Web
{
    public class MessageExtension
    {
        private static IList<string> s_availableCommandIdList = new List<string>(){};
        static MessageExtension()
        {
            s_availableCommandIdList.Add("SearchItem");
        }
        //ComposeExtensionResponse
        public static object HandleMessageExtensionQuery(ConnectorClient connector, Activity activity)
        {
            switch (activity.Name)
            {
                case "composeExtension/query":
                    {
                        var query = activity.GetComposeExtensionQueryData();
                        if (query != null && s_availableCommandIdList.Contains(query.CommandId))
                        {
                            return QueryData(connector, activity, query);
                        }
                        break;
                    }
                case "composeExtension/onCardButtonClicked":
                    {
                        var postItemInfo = JsonConvert.DeserializeObject<ItemInfo>(activity.Value.ToString());
                        var existItem = ItemDB.Items.FirstOrDefault(t => t.Id == postItemInfo.Id);
                        if (existItem != null)
                        {
                            if (postItemInfo.WantToSee)
                                existItem.WantToSeeCount++;
                            if (postItemInfo.Star)
                                existItem.StarCount++;
                        }
                        break;
                    }
                default:
                    break;
            }
            

            return null;
        }

        // Query task
        private static ComposeExtensionResponse QueryData(ConnectorClient connector, Activity activity, ComposeExtensionQuery query)
        {
            var keyword = "";
            var titleParam = query.Parameters?.FirstOrDefault(p => p.Name == "keyword");
            if (titleParam != null)
            {
                keyword = titleParam.Value.ToString();
            }

            if (string.IsNullOrEmpty(keyword))
            {
                keyword = "Microsoft";
            }

            var filteredItems = ItemDB.SearchItems(keyword);
            if (filteredItems == null || filteredItems.Count <= 0)
            {
                return new ComposeExtensionResponse(new ComposeExtensionResult("list", "result"));
            }

            var attachments = new List<ComposeExtensionAttachment>();
            foreach (var item in filteredItems)
            {
                attachments.Add(GetItemAttachment(item));
            }

            var response = new ComposeExtensionResponse(new ComposeExtensionResult("list", "result"));
            response.ComposeExtension.Attachments = attachments.ToList();

            return response;
        }

        private static ComposeExtensionAttachment GetItemAttachment(ItemInfo item)
        {
            var card = new ThumbnailCard
            {
                Title = item.Name,
                Text = item.Description.Length > 20 ? item.Description.Substring(0, 20) : item.Description,
                Images = new List<CardImage> { new CardImage(item.Image) },
                Subtitle = item.Link,
                Tap = new CardAction() { Type = "postBack", Title = item.Name, Value = "https://docs.microsoft.com/en-us/MicrosoftTeams/teams-overview", Image = item.Image },
            };

            var starBtn = new CardAction() { Type = "messageBack", DisplayText = "Star", Title = "Star", Value = "{\"Id\":\"" + item.Id + "\",\"Star\":true}" };
            var updateItemBtn = new CardAction() { Type = "invoke", DisplayText = "Update " + item.Name, Title = "Update this item", Value = "{\"type\":\"task/fetch\",\"title\":\"Update\",\"Id\":\"" + item.Id + "\"}" };
            card.Buttons = new List<CardAction>() { starBtn, updateItemBtn };


            return card
                .ToAttachment()
                .ToComposeExtensionAttachment();
        }

        private static Attachment GetHeroCard(string title = null)
        {
            var card = new ThumbnailCard
            {
                Title = !string.IsNullOrWhiteSpace(title) ? title : Faker.Lorem.Sentence(),
                Text = Faker.Lorem.Paragraph(),
                Images = new List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) },
                Subtitle = "Subtitle",
                Tap = new CardAction() { Type = "openUrl", DisplayText = "display text", Title = "tap title", Text = "tap text", Value = "http://www.baidu.com", Image = "http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString() },
                Buttons = new List<CardAction> { new CardAction() { Type = "call", DisplayText = "call Paul Cheung", Title = "will call paul", Text = "button text", Value = "15221316970", Image = "http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString() } }
            };

            return card.ToAttachment();
        }
    }
}
