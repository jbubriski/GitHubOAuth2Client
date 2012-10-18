GitHubOAuth2Client
==================

A GitHub OAuth 2 Client for ASP.NET MVC 4.

Add this to your project and add 1 line of code and it will hook directly into the Membership Provider in ASP.NET MVC 4!

## Installation

***Nuget Installation coming soon***

For now, simply:

1. Pull down the project, compile it, and add a reference to it.
2. Add your GitHub Client ID and Client Secret to your web.config
  1. `<add key="GitHub.ClientId" value= "YourClientId" />`
  2. `<add key="GitHub.ClientSecret" value= "YourClientSecret" />`
3. Add the following line to your AuthConfig.cs:

    OAuthWebSecurity.RegisterClient(new JohnnyCode.GitHubOAuth2Client(ConfigurationManager.AppSettings["GitHub.ClientId"], ConfigurationManager.AppSettings["GitHub.ClientSecret"]), "GitHub", null);


