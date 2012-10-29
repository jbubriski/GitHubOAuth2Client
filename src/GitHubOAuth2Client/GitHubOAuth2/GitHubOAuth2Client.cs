﻿// Derived from http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/fe17e7fcb5e2#src%2fMicrosoft.Web.WebPages.OAuth%2fOAuthWebSecurity.cs
namespace JohnnyCode.GitHubOAuth2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Web;
    using DotNetOpenAuth.AspNet.Clients;
    using DotNetOpenAuth.Messaging;
    using Newtonsoft.Json;

    public class GitHubOAuth2Client : OAuth2Client
    {
        #region Constants and Fields

        /// <summary>
        /// The authorization endpoint.
        /// </summary>
        private const string _authorizationEndpoint = "https://github.com/login/oauth/authorize";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string _tokenEndpoint = "https://github.com/login/oauth/access_token";

        /// <summary>
        /// The user endpoint.
        /// </summary>
        private const string _userEndpoint = "https://api.github.com/user";

        /// <summary>
        /// The _app id.
        /// </summary>
        private readonly string _appId;

        /// <summary>
        /// The _app secret.
        /// </summary>
        private readonly string _appSecret;

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
        public GitHubOAuth2Client(string appId, string appSecret)
            : base("github")
        {
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentNullException("appId");

            if (string.IsNullOrWhiteSpace(appSecret))
                throw new ArgumentNullException("appSecret");

            _appId = appId;
            _appSecret = appSecret;
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
            var builder = new UriBuilder(_authorizationEndpoint);

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
            var request = WebRequest.Create(_userEndpoint + "?access_token=" + accessToken);
            Dictionary<string, string> authData;

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream))
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(streamReader.ReadToEnd());
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
            var builder = new UriBuilder(_tokenEndpoint);

            builder.AppendQueryArgument("client_id", _appId);
            builder.AppendQueryArgument("redirect_uri", returnUrl.AbsoluteUri);
            builder.AppendQueryArgument("client_secret", _appSecret);
            builder.AppendQueryArgument("code", authorizationCode);

            using (WebClient client = new WebClient())
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