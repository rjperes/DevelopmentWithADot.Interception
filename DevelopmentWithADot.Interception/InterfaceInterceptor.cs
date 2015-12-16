using System;

namespace DevelopmentWithADot.Interception
{
	public sealed class InterfaceInterceptor : IInstanceInterceptor
	{
		private static readonly InterceptedTypeGenerator generator = new CodeDOMInterceptedTypeGenerator();

		public Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler)
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

			if (typeToIntercept.IsAssignableFrom(instance.GetType()) == false)
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

			Type interfaceProxy = generator.Generate(this, typeof(InterfaceProxy), null, typeToIntercept);

			Object newInstance = Activator.CreateInstance(interfaceProxy, this, handler, instance);

			return (newInstance);
		}

		public Boolean CanIntercept(Object instance)
		{
			return (instance.GetType().GetInterfaces().Length != 0);
		}
	}
}
