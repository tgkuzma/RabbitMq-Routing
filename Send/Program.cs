using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var message = "";
            var closeApp = false;
            Console.WriteLine("----------Sender----------");
            Console.WriteLine("Write a message to send... (include the word \"happy\" or \"sad\")");
            Console.WriteLine("...or [Enter] to exit");

            do
            {
                if (!string.IsNullOrEmpty(message))
                {

                    var factory = new ConnectionFactory() {HostName = "localhost"};
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchange: "Emotional_Messages",
                            type: "direct");

                        var isHappy = message.ToLower().IndexOf("happy", StringComparison.Ordinal) > -1;

                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "Emotional_Messages",
                            routingKey: isHappy ? "Happy" : "Sad",
                            basicProperties: null,
                            body: body);
                        Console.WriteLine(" Sent... {0}", message);
                    }
                }

                message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    closeApp = true;
                }

            } while (!closeApp);

            Environment.Exit(0);
        }
    }
}
