using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Messaging;
using RequestQ.requestQLib;

namespace RequestQ.WorkerBatchPrint
{
    class Program
    {
        static QueueType thisQType = QueueType.PrintBatchQueue;
        static QueueConfig qc;
        static ReqMessageQueue q;
        static ReqMessageQueue qOut;

        static void Main(string[] args)
        {
            //QManager qMgr = new QManager();
            //qMgr.LoadQueues(ConfigurationSettings.AppSettings);
            Process();

        }
        static void Process()
        {
            var ts = new TimeSpan(0, 0, 10);
            qc = new QueueConfig(ConfigurationSettings.AppSettings);
            q = new ReqMessageQueue(qc.queueName);
            qOut = new ReqMessageQueue(qc.queueOut);

            while (true)
            {
                try
                {
                    Message msg = q.Receive(ts);
                    
                    HandleMessage(msg);
                }
                catch (MessageQueueException e)
                { // Test to see if this was just a timeout. 
                    // If it was, just continue, there were no msgs waiting 
                    // If it wasn't, something horrible may have happened } 
                    Console.Out.WriteLine("error " + e.Message);
                }
            }
        }

        static void HandleMessage(Message msg)
        {
            msg.Label += "Processed by " + requestQLib.Utility.GetDescriptionFromEnumValue(thisQType);
            qOut.Send(msg);
            System.Threading.Thread.Sleep(2000); // 2 sec
        }
    }
}
