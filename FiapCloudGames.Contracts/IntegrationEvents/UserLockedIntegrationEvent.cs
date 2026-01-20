using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGames.Contracts.IntegrationEvents
{
    public record UserLockedIntegrationEvent
    {
        public Guid UserId { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
    }
}
