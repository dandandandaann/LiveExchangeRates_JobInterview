using RabbitMQ.Client;

namespace IonicCurrencyExchange.Services.RabbitMq;

/// <summary>
/// Provides setup and management for RabbitMQ connections, including publishing and subscribing to messages.
/// </summary>
public interface IRabbitMqSetup
{
    /// <summary>
    /// Publishes a message to the specified RabbitMQ queue.
    /// </summary>
    /// <param name="message">The message to be published.</param>
    /// <param name="queueName">The name of the queue to publish the message to.</param>
    public void Publish(string message, string queueName);

    /// <summary>
    /// Subscribes to the specified RabbitMQ queue and processes received messages using the provided callback.
    /// </summary>
    /// <param name="queueName">The name of the queue to subscribe to.</param>
    /// <param name="onMessageReceived">The callback action to process received messages.</param>
    public void Subscribe(string queueName, Action<string> onMessageReceived);
}