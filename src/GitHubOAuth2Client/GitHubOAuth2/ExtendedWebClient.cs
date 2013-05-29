using System.Net;

namespace JohnnyCode.GitHubOAuth2
{
    /// <summary>
    /// Extended WebClient. Set Encoding to UTF-8, add user-agent request header and set null proxy for faster networking
    /// </summary>
    public class ExtendedWebClient : WebClient
    {
        /// <summary>
        /// Initialize WebClient and adds user-agent requests header
        /// </summary>
        /// <param name="userAgent"></param>
        public ExtendedWebClient(string userAgent)
        {
            Proxy = null;
            Encoding = System.Text.Encoding.UTF8;
            Headers.Add("user-agent", userAgent);
        }       
    }
}
