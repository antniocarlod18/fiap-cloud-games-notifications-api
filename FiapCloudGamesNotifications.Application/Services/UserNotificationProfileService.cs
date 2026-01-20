using FiapCloudGamesNotifications.Application.Services.Interfaces;
using FiapCloudGamesNotifications.Domain.Entities;
using FiapCloudGamesNotifications.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesNotifications.Application.Services;

public class UserNotificationProfileService : IUserNotificationProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserNotificationProfileService> _logger;

    public UserNotificationProfileService(IUnitOfWork unitOfWork, ILogger<UserNotificationProfileService> logger)
    {
        this._unitOfWork = unitOfWork;
        this._logger = logger;
    }

    public async Task CreateUserNotificationProfileAsync(Guid userId, string email)
    {
        _logger.LogInformation("Creating or verifying user notification profile for {UserId}.", userId);

        var existing = await _unitOfWork.UserNotificationProfileRepo.GetByUserAsync(userId);

        if (existing != null)
        {
            _logger.LogInformation("User notification profile for {UserId} already exists. Skipping creation.", userId);
            return;
        }

        var newProfile = new UserNotificationProfile(userId, email);

        _logger.LogInformation("Adding new user notification profile for {UserId} with email {Email}.", userId, email);

        await _unitOfWork.UserNotificationProfileRepo.AddAsync(newProfile);

        await _unitOfWork.Commit();

        _logger.LogInformation("User notification profile for {UserId} created successfully.", userId);
    }
}
