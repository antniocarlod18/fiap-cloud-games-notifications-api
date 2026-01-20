using FiapCloudGamesNotifications.Api.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Builder;

namespace FiapCloudGamesNotifications.Api.Extensions;

public static class MassTransitExtensions
{
    public static WebApplicationBuilder AddMassTransitConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: builder.Environment.EnvironmentName, includeNamespace: false));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(builder.Configuration["RabbitMQ:Host"], builder.Configuration["RabbitMQ:VirtualHost"], h =>
                {
                    h.Username(builder.Configuration["RabbitMQ:UserName"]);
                    h.Password(builder.Configuration["RabbitMQ:Password"]);
                });

                cfg.UseMessageRetry(r => r.Immediate(2));
                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<CreateUserProfileConsumer>();
            x.AddConsumer<SendNotificationOrderPlacedConsumer>();
            x.AddConsumer<SendNotificationPaymentOrderProcessedConsumer>();
            x.AddConsumer<SendNotificationUserLockedConsumer>();
            x.AddConsumer<SendNotificationUserUnlockedConsumer>();
            x.AddConsumer<SendWelcomeNotificationToNewUserConsumer>();
        });

        return builder;
    }
}
