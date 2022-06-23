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

        public EmailAdaptee(string email)
        {
            this._email = email;
        }

        public void SendMessage(string message)
        {
            // Validate Inputs
            if(string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "E-mail cannot be empty");

            // Must be less than or equal 1600 characters
            if (message.Length >= 1600)
                throw new ArgumentOutOfRangeException(nameof(message), "E-mail cannot be longer than 1600 chars");

            Console.WriteLine($"[E-mail recipient: {this._email}] Body: {message}");
        }
    }
}
