using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;

namespace Microsoft.Teams.Samples.TaskModule.Web
{
    public class Bot
    {
        public static async Task<object> HandleActivity(ConnectorClient connector, Activity activity)
        {
            switch (activity.Type)
            {
                case "conversationUpdate":
                    {
                        // Get operated memeber;
                        ChannelAccount operatedMember = null;
                        var eventType = (string)((dynamic)activity.ChannelData)["eventType"];
                        switch (eventType)
                        {
                            case "teamMemberAdded":
                                {
                                    operatedMember = activity.MembersAdded[0];
                                    await SendAddOrRemoveTeamMemberMsg(connector, activity, operatedMember, true);
                                    break;
                                }
                            case "teamMemberRemoved":
                                {
                                    operatedMember = activity.MembersRemoved[0];
                                    await SendAddOrRemoveTeamMemberMsg(connector, activity, operatedMember, false);
                                    break;
                                }
                            case "teamRenamed":
                                {
                                    await SendRenameTeamMsg(connector, activity);
                                    break;
                                }
                            case "channelCreated":
                                {
                                    await SendChannelCreatedMsg(connector, activity);
                                    break;
                                }
                            case "channelRenamed":
                                {
                                    await SendChannelRenamedMsg(connector, activity);
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }
                case "messageReaction":
                    {
                        var reactionsAdded = activity.ReactionsAdded?.Where(r => r.Type == "like").ToList();
                        var reactionRemoved = activity.ReactionsRemoved?.Where(r => r.Type == "like").ToList();
                        if (reactionsAdded != null && reactionsAdded.Count > 0)
                        {
                            await SendReactiondMsg(connector, activity, reactionsAdded, true);
                        }
                        if (reactionRemoved != null && reactionRemoved.Count > 0)
                        {
                            await SendReactiondMsg(connector, activity, reactionRemoved, false);
                        }
                        break;
                    }
                case "invoke":
                    {
                        //var postItemInfo = JsonConvert.DeserializeObject<ItemInfo>(activity.Value.ToString());
                        switch (activity.Name)
                        {
                            case "task/fetch":
                                {
                                    var id = ((dynamic)activity.Value).data.Id;
                                    var wrapper = new TaskModuleTaskInfoWrapper()
                                    {
                                        task = new TaskModuleTaskInfo()
                                        {
                                            type = "continue",
                                            value = new TaskModuleTaskInfoValue()
                                            {
                                                title = "Update Item",
                                                height = 510,
                                                width = 800,
                                                url = ConfigurationManager.AppSettings["Host"] + "/UpdateItem/" + id,
                                                fallbackUrl = ConfigurationManager.AppSettings["Host"] + "/UpdateItem/" + id,
                                            }
                                        }
                                    };
                                    return wrapper;
                                }
                            default:
                                break;
                        }
                        break;
                    }
                default:
                    {
                        // Nothing to do;
                        break;
                    }
            }
            return null;
        }

        public static async Task SendAddOrRemoveTeamMemberMsg(ConnectorClient connector, Activity activity, ChannelAccount operatedMember, bool isAdd)
        {
            var replyMsg = isAdd ? "Welcome you come here" : "Goodbye~~~";

            // Bot added or removed
            if (operatedMember.Id == activity.Recipient.Id)
            {
                return;
            }

            // Fetch members to get member name;
            var members = (await connector.Conversations.GetConversationMembersAsync(activity.Conversation.Id)).ToList();
            var memberName = members.Find(m => m.Id == operatedMember.Id)?.Name;

            var replyActivity = new Activity();
            replyActivity.Text = replyMsg;
            if (!string.IsNullOrEmpty(memberName))
            {
                replyActivity.AddMentionToText(operatedMember, MentionTextLocation.PrependText, $"@{memberName}");
            }

            replyActivity.Type = "message";
            replyActivity.Conversation = new ConversationAccount() { Id = activity.Conversation.Id };

            await connector.Conversations.SendToConversationAsync(replyActivity);
        }

        public static async Task SendRenameTeamMsg(ConnectorClient connector, Activity activity)
        {
            var newTeamName = (string)((dynamic)activity.ChannelData)["team"]["name"];
            var msg = $"Caution: The name of current team has been changed to \"{newTeamName}\"";

            var replyActivity = new Activity();
            replyActivity.Text = msg;
            replyActivity.Type = "message";
            replyActivity.Conversation = new ConversationAccount() { Id = activity.Conversation.Id };

            await connector.Conversations.SendToConversationAsync(replyActivity);
        }

        public static async Task SendChannelCreatedMsg(ConnectorClient connector, Activity activity)
        {
            var channelName = (string)((dynamic)activity.ChannelData)["channel"]["name"];
            var conversationId = (string)((dynamic)activity.ChannelData)["channel"]["id"];
            var msg = $"Welcome to channel \"{channelName}\"";

            var replyActivity = new Activity();
            replyActivity.Text = msg;
            replyActivity.Type = "message";
            replyActivity.Conversation = new ConversationAccount() { Id = conversationId };

            await connector.Conversations.SendToConversationAsync(replyActivity);
        }

        public static async Task SendChannelRenamedMsg(ConnectorClient connector, Activity activity)
        {
            var channelName = (string)((dynamic)activity.ChannelData)["channel"]["name"];
            var conversationId = (string)((dynamic)activity.ChannelData)["channel"]["id"];
            var msg = $"The channel name was changed to \"{channelName}\"";

            var replyActivity = new Activity();
            replyActivity.Text = msg;
            replyActivity.Type = "message";
            replyActivity.Conversation = new ConversationAccount() { Id = conversationId };

            await connector.Conversations.SendToConversationAsync(replyActivity);
        }

        public static async Task SendReactiondMsg(ConnectorClient connector, Activity activity, IList<MessageReaction> messageReactions, bool isAdd)
        {
            var msg = string.Empty;
            if (isAdd)
            {
                msg = $"Thanks for your like";
            }
            else
            {
                msg = $"Thanks for your unlike";
            }

            // From member
            var members = (await connector.Conversations.GetConversationMembersAsync(activity.Conversation.Id)).ToList();
            var memberName = members.Find(m => m.Id == activity.From.Id)?.Name;

            var replyActivity = new Activity();
            replyActivity.Text = msg;
            replyActivity.Type = "message";
            if (!string.IsNullOrEmpty(memberName))
            {
                replyActivity.AddMentionToText(new ChannelAccount() { Id = activity.From.Id }, MentionTextLocation.PrependText, $"@{memberName}");
            }
            replyActivity.Conversation = new ConversationAccount() { Id = activity.Conversation.Id };

            await connector.Conversations.SendToConversationAsync(replyActivity);
        }

        // Task Module
        public class TaskModuleTaskInfoWrapper
        {
            public TaskModuleTaskInfo task { get; set; }
        }

        public class TaskModuleTaskInfo
        {
            public string type { get; set; }
            public TaskModuleTaskInfoValue value { get; set; }
        }

        public class TaskModuleTaskInfoValue
        {
            public string title { get; set; }
            public int height { get; set; }
            public int width { get; set; }
            public string url { get; set; }
            public string card { get; set; }
            public string fallbackUrl { get; set; }
            public string completionBotId { get; set; }
        }
    }
}
