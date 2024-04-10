using RabbitMQ.Client;
using System.Text;

namespace RabbitMQProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var senderFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "admin",
            };
            using (var senderConnection = senderFactory.CreateConnection())
            using (var senderChannel = senderConnection.CreateModel())
            {
                DeclareQueue(senderChannel);

                var message = "Hello, RabbitMQ!";
                SendMessage(senderChannel, message);

                while (true)
                {
                    Console.WriteLine("\nEnter your message (type 'exit' to quit):");
                    message = Console.ReadLine(); // Accept user input for the message content

                    if (message.ToLower() == "exit")
                        break; // Exit the loop if the user types 'exit'

                    SendMessage(senderChannel, message);
                }
            }
        }
        static void DeclareQueue(IModel channel)
        {
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        static void SendMessage(IModel channel, string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine("\n [x] Sent {0}", message);
        }
    }
}