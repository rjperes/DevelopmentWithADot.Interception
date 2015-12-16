using System;
<<<<<<< HEAD
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	using System.Runtime.Remoting.Activation;

=======
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;

namespace DevelopmentWithADot.Interception
{
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
	[Serializable]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class InterceptionContextAttribute : ContextAttribute
	{
<<<<<<< HEAD
		public InterceptionContextAttribute() : base("InterceptionContext")
		{
		}

		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			ctorMsg.ContextProperties.Add(new InterceptionProperty());
			base.GetPropertiesForNewContext(ctorMsg);
		}

		public override Boolean IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			var p = ctx.GetProperty("Interception") as InterceptionProperty;
			return (p != null);
		}

		public override bool IsNewContextOK(Context newCtx)
		{
			var p = newCtx.GetProperty("Interception") as InterceptionProperty;
			return (p != null);
=======
		private readonly Type handlerType;

		public InterceptionContextAttribute(Type handlerType) : base("InterceptionContext")
		{
			this.handlerType = handlerType;
		}

		public InterceptionContextAttribute() : this(null)
		{
		}

		public override void GetPropertiesForNewContext(IConstructionCallMessage msg)
		{
			msg.ContextProperties.Add(new InterceptionProperty(this.handlerType, msg.LogicalCallContext, msg.Properties));
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		}
	}
}
