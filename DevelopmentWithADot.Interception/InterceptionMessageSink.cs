using System;
<<<<<<< HEAD
using System.Reflection;
=======
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Activation;
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	internal sealed class InterceptionMessageSink : IMessageSink
	{
<<<<<<< HEAD
		public InterceptionMessageSink(ContextBoundObject target, IMessageSink next)
		{
			this.Target = target;
			this.NextSink = next;
		}

		public ContextBoundObject Target { get; private set; }
		public IMessageSink NextSink { get; private set; }
=======
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
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			return (this.NextSink.AsyncProcessMessage(msg, replySink));
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
<<<<<<< HEAD
			var mcm = (msg as IMethodCallMessage);
			var mrm = null as IMethodReturnMessage;

			var handler = ContextBoundObjectInterceptor.GetInterceptor(this.Target);

			if (handler != null)
			{
				var arg = new InterceptionArgs(this.Target, mcm.MethodBase as MethodInfo, mcm.Args);

				try
				{
					handler.Invoke(arg);

					if (arg.Handled == true)
					{
						mrm = new ReturnMessage(arg.Result, new object[0], 0, mcm.LogicalCallContext, mcm);
					}
				}
				catch (Exception ex)
				{
					mrm = new ReturnMessage(ex, mcm);
				}
			}

			if (mrm == null)
			{
				mrm = this.NextSink.SyncProcessMessage(msg) as IMethodReturnMessage;
			}

			return mrm;
		}

		private void PostProcess(IMethodCallMessage methodCallMessage, ref IMethodReturnMessage mrm)
		{
		}

		private void PreProcess(ref IMethodCallMessage mcm)
		{
			throw new System.NotImplementedException();
=======
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
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		}
	}
}
