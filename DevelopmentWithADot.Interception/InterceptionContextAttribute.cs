using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;

namespace DevelopmentWithADot.Interception
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class InterceptionContextAttribute : ContextAttribute
	{
		private readonly Type handlerType;

		public override void GetPropertiesForNewContext(IConstructionCallMessage msg)
		{
			msg.ContextProperties.Add(new InterceptionProperty(this.handlerType, msg.LogicalCallContext, msg.Properties));
		}

		public InterceptionContextAttribute(Type handlerType) : base("InterceptionContext")
		{
			this.handlerType = handlerType;
		}
	}
}
