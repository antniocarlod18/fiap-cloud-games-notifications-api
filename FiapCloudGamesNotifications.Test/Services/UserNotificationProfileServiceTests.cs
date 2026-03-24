using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using FiapCloudGamesNotifications.Application.Services;
using FiapCloudGamesNotifications.Domain.Repositories;
using FiapCloudGamesNotifications.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FiapCloudGamesNotifications.Test.Services
{
    public class UserNotificationProfileServiceTests
    {
        [Fact]
        public async Task CreateUserNotificationProfileAsync_Creates_When_Not_Exists()
        {
            var userId = Guid.NewGuid();
            var email = "test@example.com";

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var profileRepoMock = new Mock<IUserNotificationProfileRepository>();
            profileRepoMock.Setup(r => r.GetByUserAsync(userId)).ReturnsAsync((UserNotificationProfile?)null);
            var added = new List<UserNotificationProfile>();
            profileRepoMock.Setup(r => r.AddAsync(It.IsAny<UserNotificationProfile>())).Callback<UserNotificationProfile>(p => added.Add(p)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.UserNotificationProfileRepo).Returns(profileRepoMock.Object);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var logger = new Mock<ILogger<UserNotificationProfileService>>().Object;
            var service = new UserNotificationProfileService(unitOfWorkMock.Object, logger);

            await service.CreateUserNotificationProfileAsync(userId, email);

            Assert.Single(added);
            Assert.Equal(email, added[0].Email);
        }

        [Fact]
        public async Task CreateUserNotificationProfileAsync_Skips_When_Exists()
        {
            var userId = Guid.NewGuid();
            var email = "test@example.com";

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var profileRepoMock = new Mock<IUserNotificationProfileRepository>();
            profileRepoMock.Setup(r => r.GetByUserAsync(userId)).ReturnsAsync(new UserNotificationProfile(userId, email));
            var added = new List<UserNotificationProfile>();
            profileRepoMock.Setup(r => r.AddAsync(It.IsAny<UserNotificationProfile>())).Callback<UserNotificationProfile>(p => added.Add(p)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.UserNotificationProfileRepo).Returns(profileRepoMock.Object);
            unitOfWorkMock.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            var logger = new Mock<ILogger<UserNotificationProfileService>>().Object;
            var service = new UserNotificationProfileService(unitOfWorkMock.Object, logger);

            await service.CreateUserNotificationProfileAsync(userId, email);

            Assert.Empty(added);
        }
    }
}
