using System;

namespace DevelopmentWithADot.Interception
{
	public sealed class InterfaceInterceptor : InstanceInterceptor
	{
		private static readonly TypeGenerator generator = new CodeDOMTypeGenerator();

		public override Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler)
		{
			if (instance == null)
			{
				throw (new ArgumentNullException("instance"));
			}

			if (typeToIntercept == null)
			{
				throw (new ArgumentNullException("typeToIntercept"));
			}

			if (handler == null)
			{
				throw (new ArgumentNullException("handler"));
			}

			if (typeToIntercept.IsInstanceOfType(instance) == false)
			{
				throw (new ArgumentNullException("instance"));
			}

			if (typeToIntercept.IsInterface == false)
			{
				throw (new ArgumentNullException("typeToIntercept"));
			}

			if (this.CanIntercept(instance) == false)
			{
				throw (new ArgumentNullException("instance"));
			}

			var interfaceProxy = generator.Generate(this, typeof(InterfaceProxy), null, typeToIntercept);

			var newInstance = Activator.CreateInstance(interfaceProxy, this, handler, instance);

			return (newInstance);
		}

		public override Boolean CanIntercept(Object instance)
		{
			return (instance.GetType().GetInterfaces().Length != 0);
		}
	}
}
