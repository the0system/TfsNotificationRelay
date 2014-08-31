﻿/*
 * Tfs2Slack - http://github.com/kria/Tfs2Slack
 * 
 * Copyright (C) 2014 Kristian Adrup
 * 
 * This file is part of Tfs2Slack.
 * 
 * Tfs2Slack is free software: you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the
 * Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version. See included file COPYING for details.
 */

using DevCore.Tfs2Slack.Configuration;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.Integration.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCore.Tfs2Slack.EventHandlers
{
    class ProjectCreatedHandler : IEventHandler
    {
        private static Configuration.TextElement text = Configuration.Tfs2SlackSection.Instance.Text;

        public IList<string> ProcessEvent(TeamFoundationRequestContext requestContext, object notificationEventArgs, Configuration.BotElement bot)
        {
            var ev = (ProjectCreatedEvent)notificationEventArgs;
            if (!bot.NotifyOn.HasFlag(TfsEvents.ProjectCreated)) return null;
            var locationService = requestContext.GetService<TeamFoundationLocationService>();

            string projectUrl = String.Format("{0}/{1}/{2}",
                locationService.GetAccessMapping(requestContext, "PublicAccessMapping").AccessPoint,
                requestContext.ServiceHost.Name,
                ev.Name);

            return new [] { text.ProjectCreatedFormat.FormatWith(new { ProjectUrl = projectUrl, ProjectName = ev.Name }) };
        }
    }
}
