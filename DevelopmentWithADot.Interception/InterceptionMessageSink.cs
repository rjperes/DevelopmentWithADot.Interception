using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	sealed class InterceptionMessageSink : IMessageSink
	{
		private readonly Type handleType;
		private readonly Object instance;
		private readonly LogicalCallContext context;
		private readonly IDictionary properties;

		public InterceptionMessageSink(Object instance, Type handlerType, LogicalCallContext context, IDictionary properties, IMessageSink next)
		{
			this.context = context;
			this.properties = properties;
			this.instance = instance;
			this.handleType = handlerType;
			this.NextSink = next;
		}

		public IMessageSink NextSink
		{
			get;
			private set;
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			if (!(msg is IConstructionCallMessage) && (msg is IMethodCallMessage))
			{
				var mcm = msg as IMethodCallMessage;
				var handler = this.GetHandler();
			}

			return (this.NextSink.AsyncProcessMessage(msg, replySink));
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			if (!(msg is IConstructionCallMessage) && (msg is IMethodCallMessage))
			{
				var mcm = msg as IMethodCallMessage;
				var args = new InterceptionArgs(this.instance, mcm.MethodBase as MethodInfo, mcm.InArgs);
				var handler = this.GetHandler();

				handler.Invoke(args);

				if (args.Handled == true)
				{
					return (new InterceptionReturnMessage(args, this.context, this.properties));
				}

			}

			return (this.NextSink.SyncProcessMessage(msg));
		}

		private IInterceptionHandler GetHandler()
		{
			var handler = Activator.CreateInstance(this.handleType) as IInterceptionHandler;

			return (handler);
		}
	}
}
