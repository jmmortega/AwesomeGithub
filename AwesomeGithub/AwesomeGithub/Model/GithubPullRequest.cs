using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeGithub.Model
{
    public class GithubPullRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Description { get; set; }

        [JsonProperty("user")]
        public GithubUser User { get; set; }

        [JsonProperty("created_at")]
        public DateTime PullRequestDate { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
