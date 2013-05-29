// Derived from http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/fe17e7fcb5e2#src%2fMicrosoft.Web.WebPages.OAuth%2fOAuthWebSecurity.cs
namespace JohnnyCode.GitHubOAuth2
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using DotNetOpenAuth.AspNet.Clients;
    using DotNetOpenAuth.Messaging;
    using Newtonsoft.Json;

    /// <summary>
    /// OAuth2 Client for Github
    /// See details about Github OAuth at http://developer.github.com/v3/oauth
    /// </summary>
    public class GitHubOAuth2Client : OAuth2Client
    {
        #region Constants and Fields

        /// <summary>
        /// The authorization endpoint.
        /// </summary>
        private const string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string TokenEndpoint = "https://github.com/login/oauth/access_token";

        /// <summary>
        /// The user endpoint.
        /// </summary>
        private const string UserEndpoint = "https://api.github.com/user";

        /// <summary>
        /// The _app id.
        /// </summary>
        private readonly string _appId;

        /// <summary>
        /// The _app secret.
        /// </summary>
        private readonly string _appSecret;

        private readonly string _userAgent;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubOAuth2Client"/> class.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="userAgent">
        /// Github requires every request to have User-agent header which may be used by Github for identification so they can contact you if there are problems etc.
        /// </param>
        public GitHubOAuth2Client(string appId, string appSecret, string userAgent = "DotNetOpenAuthClient/0.1")
            : base("github")
        {
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentNullException("appId");

            if (string.IsNullOrWhiteSpace(appSecret))
                throw new ArgumentNullException("appSecret");

            _appId = appId;
            _appSecret = appSecret;
            _userAgent = userAgent;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get service login url.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>An absolute URI.</returns>
        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            var builder = new UriBuilder(AuthorizationEndpoint);

            builder.AppendQueryArgument("client_id", _appId);
            builder.AppendQueryArgument("redirect_uri", returnUrl.AbsoluteUri);

            return builder.Uri;
        }

        /// <summary>
        /// The get user data.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        /// <returns>A dictionary of profile data.</returns>
        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            using (var client = new ExtendedWebClient(_userAgent))
            {
                var data = client.DownloadString(UserEndpoint + "?access_token=" + accessToken);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            }
        }

        /// <summary>
        /// Obtains an access token given an authorization code and callback URL.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <param name="authorizationCode">
        /// The authorization code.
        /// </param>
        /// <returns>
        /// The access token.
        /// </returns>
        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var builder = new UriBuilder(TokenEndpoint);

            builder.AppendQueryArgument("client_id", _appId);
            builder.AppendQueryArgument("redirect_uri", returnUrl.AbsoluteUri);
            builder.AppendQueryArgument("client_secret", _appSecret);
            builder.AppendQueryArgument("code", authorizationCode);

            using (var client = new ExtendedWebClient(_userAgent))
            {
                var data = client.DownloadString(builder.Uri);

                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }

                var parsedQueryString = HttpUtility.ParseQueryString(data);

                return parsedQueryString["access_token"];
            }
        }

        #endregion
    }
}