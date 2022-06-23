using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.Channel;
using AvansDevOps.Person;
using Xunit;
using Xunit.Sdk;

namespace AvansDevOpsTests
{
    public class PersonTests
    {
        [Fact]
        public void User_Adds_Channel()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);

            // Act
            IChannel channelOne = channelFactory.CreateEmailChannel("tom@test.nl");
            person.AddChannel(channelOne);

            // Assert
            Assert.Contains(channelOne, person.GetChannels());
        }

        [Fact]
        public void User_Adds_Multiple_Channel()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);

            // Act
            IChannel channelOne = channelFactory.CreateEmailChannel("tom@test.nl");
            IChannel channelTwo = channelFactory.CreateEmailChannel("@Tom");
            person.AddChannel(channelOne);
            person.AddChannel(channelTwo);

            // Assert
            Assert.Contains(channelOne, person.GetChannels());
            Assert.Contains(channelTwo, person.GetChannels());
        }

        [Fact]
        public void Email_Channel_Sends_Correct_Notification_Gives_No_Exception()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            IChannel channelEmail = channelFactory.CreateEmailChannel("tom@test.nl");
            const string message = "This is a test e-mail.";

            // Act
            person.AddChannel(channelEmail);

            // Assert
            var ex = Record.Exception(() => person.SendNotification(message));
            Assert.Null(ex);
        }

        [Fact]
        public void Slack_Channel_Send_Correct_Notification_Gives_No_Exception()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            IChannel channelSlack = channelFactory.CreateSlackChannel("@Tom");
            const string message = "This is a test message.";

            // Act
            person.AddChannel(channelSlack);

            // Assert
            var ex = Record.Exception(() => person.SendNotification(message));
            Assert.Null(ex);
        }

        [Fact]
        public void Channels_Send_Correct_Notification_Gives_No_Exception()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);
            IChannel channelEmail = channelFactory.CreateSlackChannel("tom@test.nl");
            IChannel channelSlack = channelFactory.CreateSlackChannel("@Tom");
            const string message = "This is a test message.";

            // Act
            person.AddChannel(channelSlack);
            person.AddChannel(channelEmail);

            // Assert
            var ex = Record.Exception(() => person.SendNotification(message));
            Assert.Null(ex);
        }

        [Fact]
        public void Email_Channel_Send_Empty_Notification_Gives_Null_Exception()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);

            IChannel emailChannel = channelFactory.CreateEmailChannel("tom@test.nl");

            const string message = "";

            // Act
            person.AddChannel(emailChannel);

            // Assert
            Assert.Throws<ArgumentNullException>(() => person.SendNotification(message));
        }

        [Fact]
        public void Slack_Channel_Send_Empty_Notification_Gives_Null_Exception()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);

            IChannel slackChannel = channelFactory.CreateEmailChannel("@Tom");

            const string message = "";

            // Act
            person.AddChannel(slackChannel);

            // Assert
            Assert.Throws<ArgumentNullException>(() => person.SendNotification(message));
        }

        [Fact]
        public void Email_Channel_Send_Out_Of_Bounds_Notification_Gives_Out_Of_Range_Exception()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);

            IChannel emailChannel = channelFactory.CreateEmailChannel("tom@test.nl");

            string message = "";

            while (message.Length <= 1600)
            {
                message += "0";
            }

            // Act
            person.AddChannel(emailChannel);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => person.SendNotification(message));
        }


        [Fact]
        public void Slack_Channel_Send_Out_Of_Bounds_Notification_Gives_Out_Of_Range_Exception()
        {
            // Arrange
            ChannelFactory channelFactory = new ChannelFactory();
            PersonModel person = new PersonModel("Tom", ERole.Lead);

            IChannel slackChannel = channelFactory.CreateSlackChannel("@Tom");

            string message = "";

            while (message.Length <= 1600)
            {
                message += "0";
            }

            // Act
            person.AddChannel(slackChannel);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => person.SendNotification(message));
        }
    }

}
