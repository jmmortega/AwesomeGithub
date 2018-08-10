using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeGithub.Model
{
    public class GithubRepositoryResult
    {
        [JsonProperty("total_count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public List<GithubRepository> Items { get; set; } = new List<GithubRepository>();
       
    }
}
