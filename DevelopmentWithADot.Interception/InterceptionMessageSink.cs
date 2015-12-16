using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	internal sealed class InterceptionMessageSink : IMessageSink
	{
		public InterceptionMessageSink(ContextBoundObject target, IMessageSink next)
		{
			this.Target = target;
			this.NextSink = next;
		}

		public ContextBoundObject Target { get; private set; }
		public IMessageSink NextSink { get; private set; }

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			return (this.NextSink.AsyncProcessMessage(msg, replySink));
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
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
		}
	}
}
