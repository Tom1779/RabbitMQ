using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

using dotenv.net;

DotEnv.Load();

string? address = Environment.GetEnvironmentVariable("LOCAL_IP_ADDRESS");
Console.WriteLine($"{address}");

var factory = new ConnectionFactory { HostName = $"{address}" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

int num1 = 4;
int num2 = 5;
int num3;

//Creating queue in case sender hasn't started yet
await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var comm_result = message switch
    {
        "add" => num3 = num1 + num2,
        "subtract" => num3 = num1 - num2,
        "divide" => num3 = num1 / num2,
        "multiply" => num3 = num1 * num2,
        _ => num3 = -33,
    };
    Console.WriteLine($" [x] Received {message}, result is {num3}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync("hello", autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();