using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
    internal sealed class InterceptionProperty : IContextProperty, IContributeObjectSink
    {
        public string Name
        {
            get
            {
                return "Interception";
            }
        }

        public bool IsNewContextOK(Context newCtx)
        {
            var p = newCtx.GetProperty(this.Name) as InterceptionProperty;
            return p != null;
        }

        public void Freeze(Context newContext)
        {
        }

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new InterceptionMessageSink(obj as ContextBoundObject, nextSink);
        }
    }
}