using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Messaging;
using System.Data;

namespace RequestQWebSend
{
    public partial class _Default : System.Web.UI.Page
    {
        private static string queueName = @".\Private$\RequestQ";
        private static string queueNameJ = @".\Private$\RequestQ;journal";
        MessageQueue helpRequestQueue = new MessageQueue(queueName, false);
        MessageQueue helpRequestQueueJ = new MessageQueue(queueNameJ, false);

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnSend_OnClick(object sender, EventArgs e)
        {
            bool isTransactionalQueue = false;
            if (!System.Messaging.MessageQueue.Exists(queueName))
            {
                System.Messaging.MessageQueue.Create(queueName, isTransactionalQueue);
            }
            
            if (!helpRequestQueue.UseJournalQueue)
                helpRequestQueue.UseJournalQueue = true;

            SendMessage();
            GetAllMessages(); 
        }

        private void SendMessage()
        {
            System.Messaging.Message theMessage = new System.Messaging.Message(ddlPriority.SelectedValue + " Request Message Q. TimeNow is " + DateTime.Now.ToString());
           // theMessage.Formatter = new XmlMessageFormatter();

            theMessage.Label = ddlPriority.SelectedValue + " ReqQ " + DateTime.Now.ToString();

            if (ddlPriority.SelectedValue == "1") //FileProcess
                theMessage.Priority = System.Messaging.MessagePriority.Normal;
            if (ddlPriority.SelectedValue == "2")//Print Batch
                theMessage.Priority = System.Messaging.MessagePriority.High;
            if (ddlPriority.SelectedValue == "3")//File Validation
                theMessage.Priority = System.Messaging.MessagePriority.Low;

            //doesn't work
            helpRequestQueue.Send(theMessage);    

            //helpRequestQueue.Send(theMessage, MessageQueueTransactionType.Automatic);
            //or
            //helpRequestQueue.Send(theMessage, MessageQueueTransactionType.Single);
        }

        private void GetAllMessages()
        {
            DataTable messageTable = new DataTable();
            messageTable.Columns.Add("Label");
            messageTable.Columns.Add("Body");
            messageTable.Columns.Add("Status");
            messageTable.Columns.Add("Date Time");

            //Set Message Filters    
            //MessagePropertyFilter filter = new MessagePropertyFilter();
            //filter.ClearAll();
            //filter.Body = true;
            //filter.Label = true;
            //filter.Priority = true;
            //helpRequestQueue.MessageReadPropertyFilter = filter;
            helpRequestQueue.MessageReadPropertyFilter.SetAll();
            helpRequestQueueJ.MessageReadPropertyFilter.SetAll();

            //Get All Messages    
            System.Messaging.Message[] messages = helpRequestQueue.GetAllMessages();
            System.Messaging.XmlMessageFormatter stringFormatter = new System.Messaging.XmlMessageFormatter(new string[] { "System.String" });


            for (int index = 0; index < messages.Length; index++)
            {
                string test = System.Convert.ToString(messages[index].Priority);
                messages[index].Formatter = stringFormatter;
                messageTable.Rows.Add(new string[] { messages[index].Label, messages[index].Body.ToString(), "New", messages[index].ArrivedTime.ToShortTimeString() });

            }

            //Get All Messages    
            messages = helpRequestQueueJ.GetAllMessages();
            
            for (int index = 0; index < messages.Length; index++)
            {
                string test = System.Convert.ToString(messages[index].Priority);
                messages[index].Formatter = stringFormatter;
                messageTable.Rows.Add(new string[] { messages[index].Label, messages[index].Body.ToString(), "Processed", messages[index].SentTime.ToShortTimeString() });

            }

            Gridview1.DataSource = messageTable;
            Gridview1.DataBind();
        }

        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            GetAllMessages();
        }  

    }
}
