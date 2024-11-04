using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbir Sender App";

IConnection conn = factory.CreateConnection();

IModel channel = conn.CreateModel();

//direct exchange
//string exchangeName = "DemoExchange";
//string routingKey = "dmeo-routing-key";
//string queueName = "DemoQueue";

//channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
//channel.QueueDeclare(queueName, false, false, false, null);
//channel.QueueBind(queueName, exchangeName, routingKey, null);

//fanout exchange
string exchangeName = "FanOutExchangeDemo";
channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true);


for (int i = 0; i < 100; i++)
{
    Console.WriteLine($"Sending Message {i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message #{i}");
    //channel.BasicPublish(exchangeName, null, null, messageBodyBytes);

    channel.BasicPublish(exchangeName, string.Empty, null, messageBodyBytes);
    Thread.Sleep(1000);
}
channel.Close();
conn.Close();