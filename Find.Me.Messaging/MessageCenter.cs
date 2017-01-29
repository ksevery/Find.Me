using Realtime.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find.Me.Messaging
{
    public class MessageCenter
    {
        private const string ApplicationKey = "mDUiJO";

        private OrtcClient Client { get; set; }

        public MessageCenter()
        {
            this.Client = new OrtcClient();
            this.Client.Connect(ApplicationKey, string.Empty);
        }

        public event EventHandler MessageReceive;

        protected virtual void OnMessageReceive(EventArgs e)
        {
            MessageReceive?.Invoke(this, e);
        }

        public void SendMessage(string message, string channel)
        {
            Client.Send(channel, message);
        }
    }
}
