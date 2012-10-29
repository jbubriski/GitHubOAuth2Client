namespace GitHubOAuth2.Tests
{
    using System;
    using JohnnyCode.GitHubOAuth2;
    using Xunit;

    public class GitHubOAuth2ClientTests
    {
        [Fact]
        public void Ctor_WithNullAppId_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(() => { new GitHubOAuth2Client(null, "something"); });

            Assert.Equal("appId", argumentNullException.ParamName);
        }

        [Fact]
        public void Ctor_WithNullAppSecret_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(() => { new GitHubOAuth2Client("something", null); });

            Assert.Equal("appSecret", argumentNullException.ParamName);
        }

        [Fact]
        public void Ctor_SetsCorrectProviderName()
        {
            // Arrange
            var gitHubOAuth2Client = new GitHubOAuth2Client("something", "something");

            // Act
            // Assert
            Assert.Equal("github", gitHubOAuth2Client.ProviderName);
        }
    }
}
