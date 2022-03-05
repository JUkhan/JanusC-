using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JanusApp.lib.plugins
{

  public class VideoCall : PluginBase
  {

    public VideoCall()
    {
      Task.Run(() => Init());
    }
    private async Task Init()
    {
      //Simulating onSuccess
      await Task.Delay(10);
      OnSuccess();
    }
    public void Send(Payload payload)
    {
      var message = new Message(payload);
    }
    public void Send(Payload payload, object jsep)
    {
      var message = new Message(payload, jsep);
    }
    public void Data(string text)
    {

    }
    public void Data(string text, Action<Cause> error)
    {

    }
    public void Data(string text, Action<Cause> error, Action success)
    {

    }
    public void createOffer(bool media, Action<object> success, Action<Cause> error)
    {

    }

    public void createAnswer(object jsep, Action<object> success, Action<Cause> error)
    {

    }

    public event Action<object> IceState;
    public delegate void MessaheHandller(MessageData message, object jsep);
    public event MessaheHandller OnMessage;

  }
  public record MessageData(Result result, Cause error);
  public record Result(List<string> list, string events, string userName);

}
