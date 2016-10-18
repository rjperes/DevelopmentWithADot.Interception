using System;

namespace DevelopmentWithADot.Interception
{
	public sealed class TransparentProxyInterceptor : IInstanceInterceptor
	{
		public static readonly IInstanceInterceptor Instance = new TransparentProxyInterceptor();

		public object Intercept(object instance, Type typeToIntercept, IInterceptionHandler handler)
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

			if (this.CanIntercept(instance) == false)
			{
				throw (new ArgumentException("instance"));
			}

			if (typeToIntercept.IsInstanceOfType(instance) == false)
			{
				throw (new ArgumentException("typeToIntercept"));
			}

			var proxy = new TransparentProxy(this, instance, typeToIntercept, handler);

			return (proxy.GetTransparentProxy());
		}

		public bool CanIntercept(object instance)
		{
			return ((instance is MarshalByRefObject) || (instance.GetType().GetInterfaces().Length != 0));
		}
	}
}
