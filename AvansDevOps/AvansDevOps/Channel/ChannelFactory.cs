using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using AvansDevOps.Person;
using System.Collections.Generic;

namespace AvansDevOps.Channel
{
    public class ChannelFactory
    {
        public IChannel CreateEmailChannel(string email)
        {
            return new EmailChannel(email);
        }


        public IChannel CreateSlackChannel(string username)
        {
            return new SlackChannel(username);
        }
    }
}
