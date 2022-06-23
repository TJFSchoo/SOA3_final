using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Channel
{
    public class SlackAdaptee
    {
        private readonly string _username;
        private static string TAG = "SlackAdaptee";

        public SlackAdaptee(string username)
        {
            this._username = username;
        }

        public void SendMessage(string message)
        {
            // Validate Inputs
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), 
                    "[" + TAG + "] " + "Body can't be empty.");

            // Must be less than or equal 1600 characters
            if (message.Length >= 1000)
                throw new ArgumentOutOfRangeException(nameof(message), 
                    "[" + TAG + "] " + "Body can't be longer than 1000 characters.");

            Console.WriteLine($"[Slack message recipient: {this._username}] Body: {message}");
        }
    }
}
