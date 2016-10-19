using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;

namespace DevelopmentWithADot.Interception
{
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

		public override bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			var p = ctx.GetProperty(InterceptionProperty.InterceptionPropertyName) as InterceptionProperty;
			return (p != null);
		}

		public override bool IsNewContextOK(Context newCtx)
		{
			var p = newCtx.GetProperty(InterceptionProperty.InterceptionPropertyName) as InterceptionProperty;
			return (p != null);
		}
	}
}
