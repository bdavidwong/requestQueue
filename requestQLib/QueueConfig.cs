using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Configuration;

namespace RequestQ.requestQLib
{
    public class QueueConfig
    {
        public NameValueCollection values { get; private set; }
        public QueueConfig(NameValueCollection AppSettings)
        {
                values = AppSettings;
                queueName = AppSettings[AppSettings["queueName"].ToString()].ToString();
                queueFileProcess = AppSettings["queueFileProcess"].ToString();
                queuePrintBatch = AppSettings["queuePrintBatch"].ToString();
                queueFileValidation = AppSettings["queueFileValidation"].ToString();
                queueRequest = AppSettings["queueRequest"].ToString();
                queueOut = AppSettings["queueOut"].ToString();

            //foreach (string key in AppSettings)
            //{
            //    string value = AppSettings[key];
            //    if (value.IndexOf("queues") > 0)
            //    {
            //        ReqMessageQueue q = new ReqMessageQueue(value);
            //        if (q != null)
            //            queues.Add(q);
            //    }
            //}
            //    ConfigurationManager.AppSettings["queueName"].ToString(),
            //    ConfigurationManager.AppSettings["queueFileProcess"].ToString(),
            //    ConfigurationManager.AppSettings["queuePrintBatch"].ToString(),
            //    ConfigurationManager.AppSettings["queueFileValidation"].ToString());
        }

        public string GetQueueName(QueueType qType)
        {
            if (qType == QueueType.FileProcessQueue)
                return "queueFileProcess";
            if (qType == QueueType.PrintBatchQueue)
                return "queuePrintBatch";
            if (qType == QueueType.ValidationQueue)
                return "queueFileValidation";
            if (qType == QueueType.QueueOut)
                return "queueOut";
            if (qType == QueueType.RequestQueue)
                return "queueRequest";
            else
                return string.Empty;

        }
        public string queueName { get; set; }
        public string queueFileProcess { get; set; }
        public string queuePrintBatch { get; set; }
        public string queueFileValidation { get; set; }
        public string queueRequest { get; set; }
        public string queueOut { get; set; }
    }
}
