using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace M2Mqtt.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MqttClient(Constants.Server);

            client.Subscribe(new[] { Constants.OutTopic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            client.Connect(Guid.NewGuid().ToString(), Constants.UserName, Constants.Password);
            int ledState = 0;

            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived; ;

            while (true)
            {
                ledState = 1;
                client.Publish(Constants.InTopic, Encoding.UTF8.GetBytes(ledState.ToString()));
                Thread.Sleep(1000);
                ledState = 0;

                client.Publish(Constants.InTopic, Encoding.UTF8.GetBytes(ledState.ToString()));
                Thread.Sleep(1000);
            }
          
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine($"Received - {message}");
        }
    }
}
