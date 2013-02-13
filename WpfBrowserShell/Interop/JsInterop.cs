using Newtonsoft.Json;
using WpfBrowserShell.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfBrowserShell.Interop
{
    public interface IJsInterop
    {
        void Ready(string callback);
        void NotifyWPF(string json);
    }

    public class InteropMessage
    {
        public string Type { get; set; }
        public string Payload { get; set; }
    };

    public class JsStatus : EventArgs
    {
        public int[] Selected { get; set; }
        public int Completed { get; set; }
        public int Total { get; set; }
    };

    [ComVisibleAttribute(true)]
    public class JsInterop : IJsInterop
    {
        public event EventHandler<EventArgs> JsReady;
        public event EventHandler<JsStatus>  Status;

        public Action<string, string> InvokeJs { get; set; }

        public void Ready(string callback)
        {
            CallbackName = callback;
            RaiseReady();
        }

        public void NotifyWPF(string json)
        {
            var message = JsonConvert.DeserializeObject<InteropMessage>(json);
            // Handle notifications via some sort of factory?
            switch( message.Type )
            {
                case "status":
                    var payload = JsonConvert.DeserializeObject<JsStatus>(message.Payload);
                    RaiseStatus(payload);
                    break;
            }
        }

        public void NotifyJs(string type, string json)
        {
            if (CallbackName != null && InvokeJs != null)
            {
                var message = new InteropMessage()
                {
                    Type = type,
                    Payload = json
                };
                InvokeJs(CallbackName, JsonConvert.SerializeObject(message));
            }
        }

        string CallbackName
        {
            get;
            set;
        }

        void RaiseReady()
        {
            if (JsReady != null)
                JsReady(this, new EventArgs());
        }

        void RaiseStatus(JsStatus status)
        {
            if (Status != null)
                Status(this, status);
        }
    }

}
