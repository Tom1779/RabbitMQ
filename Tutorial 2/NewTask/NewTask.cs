using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

//creating queue
await channel.QueueDeclareAsync(queue: "hello", durable: true, exclusive: false, autoDelete: false,
    arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

int op = 0;

var message = GetMessage(op);
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
Console.WriteLine($" [x] Sent {message}");

op++;

message = GetMessage(op);
body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();


static string GetMessage(int op)
{
    int first_num = 11;
    int second_num = 10;

    if (op == 0){
        return $"{first_num} + {second_num} = {first_num + second_num}";
    }

    return $"{first_num} * {second_num} = {first_num * second_num}";
    
}