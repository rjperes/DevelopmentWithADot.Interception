using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	using System.Runtime.Remoting.Activation;

	[Serializable]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class InterceptionContextAttribute : ContextAttribute, IContributeClientContextSink, IContributeServerContextSink, IContributeObjectSink
	{
		public InterceptionContextAttribute() : base("InterceptionContext")
		{
		}

		public override Boolean IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			return (base.IsContextOK(ctx, ctorMsg));
		}

		public IMessageSink GetClientContextSink(IMessageSink nextSink)
		{
			return (new InterceptionMessageSink(nextSink));
		}

		public IMessageSink GetServerContextSink(IMessageSink nextSink)
		{
			return (new InterceptionMessageSink(nextSink));
		}

		public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
		{
			return (new InterceptionMessageSink(nextSink));
		}
	}
}
