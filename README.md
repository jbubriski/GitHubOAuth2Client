GitHubOAuth2Client
==================

A GitHub OAuth 2 Client for ASP.NET MVC 4 via DotNetOpenAuth.

Add this to your project and add 1 line of code and it will hook directly into the Membership Provider in ASP.NET MVC 4!

Auto-magical-fantastical Installation
-------------------------

[Use Nuget!](https://nuget.org/packages/GitHubOAuth2Client/0.1.0.0)

Manual-idiotic-stupid Installation
------------------

1. Pull down the project, compile it, and add a reference to it.
2. Add your GitHub Client ID and Client Secret to your web.config
  1. `<add key="GitHub.ClientId" value= "YourClientId" />`
  2. `<add key="GitHub.ClientSecret" value= "YourClientSecret" />`
3. Add the following line to your AuthConfig.cs:

    ```csharp
    OAuthWebSecurity.RegisterClient(new JohnnyCode.GitHubOAuth2Client(ConfigurationManager.AppSettings["GitHub.ClientId"], ConfigurationManager.AppSettings["GitHub.ClientSecret"]), "GitHub", null);
    ```


