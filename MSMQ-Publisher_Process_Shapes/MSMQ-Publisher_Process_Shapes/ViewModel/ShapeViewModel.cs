using MSMQ.Messaging;
using MSMQ_Publisher_Process_Shapes.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Windows.Shapes;

namespace MSMQ_Publisher_Process_Shapes.ViewModel
{
    public class ShapeViewModel
    {
        readonly string privateQueuePath = ConfigurationManager.AppSettings["PrivateQueuePath"] ?? "default value";

        public ShapeViewModel()
        {
        }

        public void Publish(Shape shape, double x, double y)
        {
            SendDataToQueue();
            MessageQueue queue = new(privateQueuePath);
            var shapeData = new ShapeData()
            {
                X = x,
                Y = y,
                Height = shape.Height,
                Width = shape.Width,
                ShapeType = shape.GetType(),
                Stroke = shape.Stroke,
                Fill = shape.Fill
            };

            string message = JsonConvert.SerializeObject(shapeData);
            Message msg = new Message(message);
            queue.Send(msg, "ShapeQueue");
        }

        private void SendDataToQueue()
        {
            if (!MessageQueue.Exists(privateQueuePath)) MessageQueue.Create(privateQueuePath);
        }
    }
}
