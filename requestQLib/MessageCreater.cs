using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace RequestQ.requestQLib
{
    public class MessageCreater
    {
        private string messageBody;

        public MessageCreater(string data)
        {
            messageBody = data;
        }

        public Message CreateMeesage(QueueType qType)
        {

            Message theMessage = new Message(messageBody);
            theMessage.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "System.String" });

            theMessage.Label = ((int)qType).ToString() + "|" + Guid.NewGuid();

            return theMessage;
        }
    }
}
