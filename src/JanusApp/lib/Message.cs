using System;
namespace JanusApp.lib
{
    public enum Request
    {
        Register, Call, Hangup, List
    }
    public class Payload
    {
        private readonly Request request;
        private readonly string userName;
       
        public Payload(Request request)
        {
            this.request = request;
           
        }
        public Payload(Request request, string userName)
        {
            this.request = request;
            this.userName = userName;
        }
       
    }
    public class Message
    {
        private readonly Payload message;
        private readonly object jsep;

        public Message(Payload message)
        {
            this.message = message; 
        }
        public Message(Payload message, object jsep)
        {
            this.message = message;
            this.jsep = jsep;
        }

    }

}
