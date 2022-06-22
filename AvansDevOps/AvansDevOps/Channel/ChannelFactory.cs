using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Person;

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
