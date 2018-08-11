using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeGithub.Model
{
    public class GithubRepository
    {
        [JsonProperty("name")]
        public string RepositoryName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("avatar_url")]
        public string Avatar { get; set; }

        [JsonProperty("name")]
        public string Username { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("forks_count")]
        public int ForksCount { get; set; }

        [JsonProperty("stargazers_count")]
        public int StarsCount { get; set; }

        [JsonProperty("owner")]
        public GithubOwner Owner { get; set; }
            

    }
}
