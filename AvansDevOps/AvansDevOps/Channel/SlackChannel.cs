using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Channel
{
    public class SlackChannel : IChannel
    {
        private readonly string _username;
        private readonly SlackAdaptee _adaptee;

        public SlackChannel(string username)
        {
            this._username = username;
            this._adaptee = new SlackAdaptee(this._username);
        }

        public void SendMessage(string message)
        {
            this._adaptee.SendMessage(message);
        }
    }
}
