using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


ConnectionFactory factory = new();
factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbir Receiver2 App";

IConnection conn = factory.CreateConnection();

IModel channel = conn.CreateModel();

//for direct exchange
//string exchangeName = "DemoExchange";
//string routingKey = "dmeo-routing-key";
//string queueName = "DemoQueue";

//channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
//channel.QueueDeclare(queueName, false, false, false, null);
//channel.QueueBind(queueName, exchangeName, routingKey, null);

//fanout exchange
string exchangeName = "FanOutExchangeDemo";
string queueName = "FanOutExchangeDemo2";

channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true);
channel.QueueDeclare(queueName, true, false, false, null);
channel.QueueBind(queueName, exchangeName, string.Empty);

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(3)).Wait();
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message Received: {message}");

    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queueName, false, consumer);

Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();
conn.Close();