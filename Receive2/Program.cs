using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("----------Sad Receiver----------");
            Console.WriteLine("Press [Enter] to exit.");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "Emotional_Messages",
                    type: "direct");
                var queueName = channel.QueueDeclare().QueueName;


                channel.QueueBind(queue: queueName,
                    exchange: "Emotional_Messages",
                    routingKey: "Sad");

                Console.WriteLine("Waiting for messages...");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" Received {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                    noAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}