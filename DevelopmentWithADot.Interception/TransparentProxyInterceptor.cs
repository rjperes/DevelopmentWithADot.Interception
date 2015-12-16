using System;

namespace DevelopmentWithADot.Interception
{
<<<<<<< HEAD
	public sealed class TransparentProxyInterceptor : IInstanceInterceptor
	{
		public Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler)
=======
	public sealed class TransparentProxyInterceptor : InstanceInterceptor
	{
		public override Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler)
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
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

<<<<<<< HEAD
			if (typeToIntercept.IsAssignableFrom(instance.GetType()) == false)
=======
			if (typeToIntercept.IsInstanceOfType(instance) == false)
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
			{
				throw (new ArgumentException("typeToIntercept"));
			}

<<<<<<< HEAD
			TransparentProxy proxy = new TransparentProxy(this, instance, typeToIntercept, handler);
=======
			var proxy = new TransparentProxy(this, instance, typeToIntercept, handler);
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

			return (proxy.GetTransparentProxy());
		}

<<<<<<< HEAD
		public Boolean CanIntercept(Object instance)
=======
		public override Boolean CanIntercept(Object instance)
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		{
			return ((instance is MarshalByRefObject) || (instance.GetType().GetInterfaces().Length != 0));
		}
	}
}
