using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace IonicCurrencyExchange.Services.RabbitMq;

/// <summary>
/// Provides setup and management for RabbitMQ connections, including publishing and subscribing to messages.
/// </summary>
public class RabbitMqSetup : IDisposable, IRabbitMqSetup
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMqSetup> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqSetup"/> class.
    /// </summary>
    /// <param name="configuration">The configuration settings for RabbitMQ.</param>
    /// <param name="logger">The logger instance for logging errors and information.</param>
    /// <exception cref="Exception">Thrown when an error occurs while connecting to RabbitMQ.</exception>
    public RabbitMqSetup(IConfiguration configuration, ILogger<RabbitMqSetup> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:HostName"],
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"]
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while connecting to RabbitMQ.");
        }
    }

    /// <summary>
    /// Publishes a message to the specified RabbitMQ queue.
    /// </summary>
    /// <param name="message">The message to be published.</param>
    /// <param name="queueName">The name of the queue to publish the message to.</param>
    public void Publish(string message, string queueName)
    {
        try
        {
            _channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while publishing a message to RabbitMQ.");
        }
    }

    /// <summary>
    /// Subscribes to the specified RabbitMQ queue and processes received messages using the provided callback.
    /// </summary>
    /// <param name="queueName">The name of the queue to subscribe to.</param>
    /// <param name="onMessageReceived">The callback action to process received messages.</param>
    public void Subscribe(string queueName, Action<string> onMessageReceived)
    {
        try
        {
            _channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                onMessageReceived(message);
            };

            _channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while subscribing to a RabbitMQ queue.");
        }
    }

    /// <summary>
    /// Disposes the RabbitMQ connection and channel.
    /// </summary>
    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}