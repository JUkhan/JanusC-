using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JanusApp.lib.plugins;

namespace JanusApp.lib
{
    public class Janus
    {
        public event Action Success;
        public event Action<Cause> Error;
        public event Action Destroy;

        private string server;
        private string sessionId;
        private Dictionary<string, Janus> sessions=new Dictionary<string, Janus>();
        private bool isConnected=false;
        public Janus(string server)
        {
            this.server = server;
            Task.Run(() =>TryConnectToTheServer());
        }
        private async Task<bool> TryConnectToTheServer()
        {
            try {
                var conn = await HttpApiCall();
                isConnected = true;
                sessionId =conn.session_id;
                sessions[sessionId] = this;
                OnSuccess();
            }catch(Exception ex) {
                OnError(ex.Message);
            }
            return true;
        }

        private Task<ConnectionResult> HttpApiCall()
        {
            return Task.FromResult(new ConnectionResult("", session_id: "uniqueid"));
        }
        public T Attach<T>() where T : PluginBase, new()
        {
            if (!isConnected) OnError("Server not connected");
            return new T();
        }
        protected virtual void OnSuccess()
        {
            if (!isConnected) OnError(("Server not connected"));
            else if (Success != null) Success();
        }

        protected virtual void OnError(string reason)
        {
            if (Error != null) Error(new Cause(reason));
        }

        protected virtual void OnDestroy()
        {
            if (Destroy != null) Destroy();
        }

        public static void Init(bool debug, object dependencies, Action callback)
        {
            // Todo some settings
            callback();
        }
        public void DestroySession(string id)
        {
            var key = string.IsNullOrEmpty(id) ? sessionId : id;
            if (sessions.ContainsKey(key))
            {
                sessions.Remove(key);
                OnDestroy();
            }
        }

    }

    public record Cause(string message);
    public record ConnectionResult(string id, string session_id);
}
