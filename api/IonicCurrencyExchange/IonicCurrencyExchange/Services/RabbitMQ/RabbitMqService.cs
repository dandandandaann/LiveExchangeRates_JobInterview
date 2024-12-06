using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace IonicCurrencyExchange.Services.RabbitMq
{
    public class RabbitMqService : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(IConfiguration configuration, ILogger<RabbitMqService> logger)
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

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}