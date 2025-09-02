using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

string[] commands = { "add", "subtract", "multiply", "divide" };
Random rand_comm = new Random();
int command = rand_comm.Next(0, 4); 

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

//creating queue
await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
    arguments: null);

string message = $"{commands[command]}";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();