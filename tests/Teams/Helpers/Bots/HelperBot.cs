﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class HelperBot : TeamsActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            turnContext.Activity.RemoveRecipientMention();

            if (turnContext.Activity.Text == "notify")
            {
                var msg = MessageFactory.Text("This message will contain a notification");
                msg.NotifyUser();

                await turnContext.SendActivityAsync(msg, cancellationToken);
            }
            else if (turnContext.Activity.Text == "random")
            {
                var channels = await GetChannelsAsync(turnContext, cancellationToken);
                Random random = new Random();
                var channel = random.Next(0, channels.Count);
                var channelName = channels[channel].Name == null ? "General" : channels[channel].Name;

                var msg = MessageFactory.Text($"I will send this to the {channelName} channel");

                await turnContext.SendActivityAsync(msg, cancellationToken);
                await turnContext.TeamsSendToChannelAsync(channels[channel].Id, msg, cancellationToken);
            }
            else if (turnContext.Activity.Text == "general")
            {
                var msg = MessageFactory.Text("This message will appear in the team's general channel");
                await turnContext.TeamsSendToGeneralChannelAsync(msg, cancellationToken);
            }
            else
            {
                var msg = MessageFactory.Text("Send me \"notify\" to get a notificaiton. Send me \"random\" to send a message to a random channel." +
                    "Send me \"general\" to send a message to the general channel");
                await turnContext.SendActivityAsync(msg, cancellationToken);
            }
        }
    }
}
