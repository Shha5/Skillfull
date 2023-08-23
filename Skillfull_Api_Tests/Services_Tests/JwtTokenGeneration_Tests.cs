using SkillfullAPI.Services;
using Moq;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Identity;
using SkillfullAPI.Models.AuthModels;
using SkillfullAPI.Services.Interfaces;

namespace Skillfull_Api_Tests.Services_Tests
{
    [TestFixture]
    internal class JwtTokenGenerationService_Tests : ApiTestsBase<TokenGenerationService>
    {

        //private List<RefreshTokenModel> testRefreshTokens = new List<RefreshTokenModel>()
        //{
        //    new RefreshTokenModel
        //    {
        //        Id = 1,
        //        UserId = "29828274-0adc-4bff-98dc-5ade56c3146c",
        //        Token = "WyYK4vLll4Pu999V2djbaOA",
        //        JwtId = "77c08774-4145-4b08-bc03-b71ffc3a87c8",
        //        IsUsed = false,
        //        IsRevoked = false,
        //        AddedDate = DateTime.UtcNow,
        //        ExpiryDate = DateTime.UtcNow.AddDays(1)
        //    },
        //     new RefreshTokenModel
        //    {
        //        Id = 2,
        //        UserId = "94ce4e68-3011-4b89-8416-537fb127d2b8",
        //        Token = "UUJV3x0qlycrG3MymFXhFry",
        //        JwtId = "0a11c1f7-eea8-43c3-ad3e-b00b0ed4824c",
        //        IsUsed = true,
        //        IsRevoked = false,
        //        AddedDate = DateTime.UtcNow,
        //        ExpiryDate = DateTime.UtcNow.AddDays(1)
        //    },
        //      new RefreshTokenModel
        //    {
        //        Id = 3,
        //        UserId = "aKcJl8rNaBcYMgpyl3Z1a3z",
        //        Token = "7343bb9d-070b-4cbe-a676-ada7945f46e1",
        //        JwtId = "a7b9e3fa-682c-4290-9ec2-6b1d670db772",
        //        IsUsed = false,
        //        IsRevoked = false,
        //        AddedDate = DateTime.UtcNow.AddDays(-2),
        //        ExpiryDate = DateTime.UtcNow.AddDays(-1)
        //    },

        //};


        [Test]
        public async Task GenerateJwtToken_UserIsValidConfigIsValid_ReturnsAuthResultWithTokens()
        {
            var mockUser = AutoMock.Mock<IdentityUser>().SetupAllProperties();
            mockUser.Object.Email = "example@example.com";
            mockUser.Object.Id = "38e29bf0-1b02-468c-8fa3-51a6e5df72ae";
            var dataAccessMock = new Mock<IDataAccessService>().Setup(d => d.SaveRefreshToken(It.IsAny<RefreshTokenModel>())).Returns(Task.CompletedTask);
            ConfigMock.Setup(c => c.GetSection("JwtConfig:Secret").Value).Returns("XulEF7VgLBXkbGnStR8Wt277EmjqjDGl9rS");
            ConfigMock.Setup(c => c.GetSection("JwtConfig:ExpiryTimeFrame").Value).Returns("20");

            var result = await SystemUnderTest.GenerateJwtToken(mockUser.Object);

            Assert.IsTrue(result.Result);
            Assert.That(result.Token.Length, Is.Positive);
            Assert.That(result.RefreshToken.Length, Is.Positive);

        }

        //[Test]
        //public async Task GenerateJwtToken_ConfigIsNotValid_Re
    }
}
