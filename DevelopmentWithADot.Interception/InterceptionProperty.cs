using System;
using System.Collections;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	sealed class InterceptionProperty : IContextProperty, IContributeObjectSink
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
