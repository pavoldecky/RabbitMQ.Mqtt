using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbit.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.UserName = Constants.UserName;
            factory.Password = Constants.Password;
            factory.VirtualHost = "/";
            factory.Protocol = Protocols.DefaultProtocol;
            factory.HostName = Constants.Server;
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                var routingKey = Constants.InTopic;
                int ledState = 0;
                string message = null;
                while(true)
                {
                    ledState = 1;
                    message = ledState.ToString();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "amq.topic",
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                    Thread.Sleep(1000);

                    ledState = 0;
                    message = ledState.ToString();
                    body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "amq.topic",
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                    Thread.Sleep(1000);
                }
               
            }
        }
    }
}
