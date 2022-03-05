using System;
using System.Threading;
using JanusApp.lib;
using JanusApp.lib.plugins;

namespace JanusApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Janus video call");

            Action<VideoCall> registerUser = vc =>
            {
                Console.WriteLine("Register user");
                var payload = new Payload(Request.Register, userName: "jasim");
                vc.Send(payload);
            };

            Action<VideoCall> doCall = vc =>
            {
                Console.WriteLine("dialling...");
                vc.createOffer(
                    media: true,
                    success: jsep => {
                        vc.Send(new Payload(Request.Call, userName:"ripon"), jsep);
                    },
                    error: err => Console.WriteLine(err.message)
                 );
            };

            Janus.Init(debug:true, dependencies: null, callback: () =>
            {
                Console.WriteLine("Set up success");
                var janus = new Janus("Server url");

                janus.Success += () => {
                    Console.WriteLine("server is connected");
                   //attach a video call plugin
                    var vc = janus.Attach<VideoCall>();
                    
                    vc.Success += () =>
                    {
                        registerUser(vc);
                        doCall(vc);
                    };
                    vc.IceState += state => { };
                    vc.OnMessage += (message, jsep) =>
                    {
                        if(message.result is Result)
                        {
                            if (message.result.list.Count > 0)
                            {
                                //Got List of registered users
                            }
                            else
                            {
                                switch (message.result.events)
                                {
                                    case "registered":
                                        var username = message.result.userName;
                                        //Get a list of available peers
                                        vc.Send(new Payload(Request.List));
                                        break;
                                    case "calling":
                                        //waiting foor the peer to answer...
                                        break;
                                    case "incomingcall":
                                        //incoming call from + message.result.userName;
                                        vc.createAnswer(jsep, jsep =>
                                        {
                                            vc.Send(new Payload(Request.Call), jsep);

                                        }, err =>Console.WriteLine(err.message));
                                        break;
                                    case "accepted":
                                        
                                        break;
                                    case "hangup":

                                        break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.error.message);
                        }
                    };
                    vc.Error += err => Console.WriteLine(err.message);
                };
                janus.Error += err => Console.WriteLine(err.message);
                janus.Destroy += () =>
                {
                    //Todo clean up code gooes here
                };

            });

            Thread.Sleep(1000);
        }
    }
}
