using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
<<<<<<< HEAD
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
=======
	internal sealed class InterceptionProperty : IContextProperty, IContributeObjectSink
	{
		private readonly Type handlerType;
		private readonly LogicalCallContext context;
		private readonly IDictionary properties;

		public InterceptionProperty(Type handlerType, LogicalCallContext context, IDictionary properties)
		{
			this.context = context;
			this.properties = properties;
			this.handlerType = handlerType;
		}

		#region IContextProperty Members

		public void Freeze(Context newContext)
		{
		}

		public Boolean IsNewContextOK(Context newCtx)
		{
			return (true);
		}

		public String Name
		{
			get
			{
				return(Guid.NewGuid().ToString());
			}
		}

		#endregion

		#region IContributeObjectSink Members

		public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
		{
			return (new InterceptionMessageSink(obj, this.handlerType, this.context, this.properties, nextSink));
		}

		#endregion
	}
}
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
