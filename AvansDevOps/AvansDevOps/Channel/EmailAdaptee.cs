using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Channel
{
    public class EmailAdaptee
    {
        private readonly string _email;
        private static string TAG = "EmailAdaptee";

        public EmailAdaptee(string email)
        {
            this._email = email;
        }

        public void SendMessage(string message)
        {
            // Must have content
            if(string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), 
                    "[" + TAG + "] " + "Body can't be empty.");

            // Less than or equal 1000 characters
            if (message.Length >= 1000)
                throw new ArgumentOutOfRangeException(nameof(message), 
                    "[" + TAG + "] " + "Body can't be longer than 1000 characters.");

            Console.WriteLine($"[E-mail recipient: {this._email}] Body: {message}");
        }
    }
}
