using System;
using System.Text;
//using RabbitMQ.Client;
using ABB.Sensors.Motion;

namespace ABB.Services.Motion
{
    class MotionSensorRabbitMQPublisher
    {
        private IMotionSensor motionSensor;

        public MotionSensorRabbitMQPublisher()
        {
            InitializeQueue();

            InitializeSensor();
        }

        private void InitializeSensor()
        {
            motionSensor = MotionSensorFactory.Create();
            motionSensor.InitGPIO();
            motionSensor.MotionDetected += MotionSensor_MotionDetected;
            motionSensor.MotionUndetected += MotionSensor_MotionUndetected;
        }

        private void MotionSensor_MotionUndetected(IMotionSensor sender, string args)
        {
            Publish("Motion undetected at " + DateTime.Now);
        }
        
        private void MotionSensor_MotionDetected(IMotionSensor sender, string e)
        {
            Publish("Motion detected at " + DateTime.Now);
        }


        private static void InitializeQueue()
        {
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare(queue: "MotionSensor",
            //                        durable: false,
            //                        exclusive: false,
            //                        autoDelete: false,
            //                        arguments: null);
            //    }
            //}
        }

        public void Publish(string message)
        {
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {                    
            //        var body = Encoding.UTF8.GetBytes(message);

            //        channel.BasicPublish(exchange: "",
            //                             routingKey: "MotionSensor",
            //                             basicProperties: null,
            //                             body: body);
                    
            //    }
            //}
        }
    }
}
