using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	using System.Runtime.Remoting.Activation;

	[Serializable]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class InterceptionContextAttribute : ContextAttribute
	{
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
		}
	}
}
