﻿using AwesomeGithub.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeGithub.Extension
{
    public static class ExtensionMethodsGithub
    {
        public static Tuple<string,string> PullRequestParams(this GithubRepository githubRepository)
        {
            //Also if not working you can get  "pulls_url" field from Json
            return new Tuple<string, string>(githubRepository.Owner.Login, githubRepository.RepositoryName);
        }        
    }
}
