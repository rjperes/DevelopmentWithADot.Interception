using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	internal sealed class InterceptionMessageSink : IMessageSink
	{
		private readonly Type handlerType;
		private readonly MarshalByRefObject instance;
		private readonly LogicalCallContext context;
		private readonly IDictionary properties;

		public InterceptionMessageSink(MarshalByRefObject instance, Type handlerType, LogicalCallContext context, IDictionary properties, IMessageSink next)
		{
			this.context = context;
			this.properties = properties;
			this.instance = instance;
			this.handlerType = handlerType;
			this.NextSink = next;
		}

		public IMessageSink NextSink
		{
			get;
			private set;
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			return (this.NextSink.AsyncProcessMessage(msg, replySink));
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			if (ContextBoundObjectInterceptor.IsIntercepted(this.instance) == true)
			{
				if (!(msg is IConstructionCallMessage) && (msg is IMethodCallMessage))
				{
					var mcm = msg as IMethodCallMessage;
					var args = new InterceptionArgs(this.instance, mcm.MethodBase as MethodInfo, mcm.InArgs);
					var handler = this.GetHandler();

					if (handler != null)
					{
						handler.Invoke(args);

						if (args.Handled == true)
						{
							return (new InterceptionReturnMessage(args, this.context, this.properties));
						}
					}
				}
			}

			return (this.NextSink.SyncProcessMessage(msg));
		}

		private IInterceptionHandler GetHandler()
		{
			var handler = null as IInterceptionHandler;

			if (ContextBoundObjectInterceptor.interceptors.TryGetValue(this.instance, out handler) == false)
			{
				if (handlerType != null)
				{
					handler = Activator.CreateInstance(this.handlerType) as IInterceptionHandler;
				}
			}

			return (handler);
		}
	}
}
