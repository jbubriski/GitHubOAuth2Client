using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JohnnyCode;

namespace GitHubOAuth2.Tests
{
    public class GitHubOAuth2ClientTests
    {
        public void Ctor_WithNullAppId_ThrowsException()
        {
            // Arrange

            // Act
            var gitHubOAuth2Client = new GitHubOAuth2Client(null, "something");

            // Assert

        }
    }
}
