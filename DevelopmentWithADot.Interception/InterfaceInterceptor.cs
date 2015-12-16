using System;

namespace DevelopmentWithADot.Interception
{
<<<<<<< HEAD
	public sealed class InterfaceInterceptor : IInstanceInterceptor
	{
		private static readonly InterceptedTypeGenerator generator = new CodeDOMInterceptedTypeGenerator();

		public Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler)
=======
	public sealed class InterfaceInterceptor : InstanceInterceptor
	{
		private static readonly TypeGenerator generator = new CodeDOMTypeGenerator();

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

<<<<<<< HEAD
			if (typeToIntercept.IsAssignableFrom(instance.GetType()) == false)
=======
			if (typeToIntercept.IsInstanceOfType(instance) == false)
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
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

<<<<<<< HEAD
			Type interfaceProxy = generator.Generate(this, typeof(InterfaceProxy), null, typeToIntercept);

			Object newInstance = Activator.CreateInstance(interfaceProxy, this, handler, instance);
=======
			var interfaceProxy = generator.Generate(this, typeof(InterfaceProxy), null, typeToIntercept);

			var newInstance = Activator.CreateInstance(interfaceProxy, this, handler, instance);
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

			return (newInstance);
		}

<<<<<<< HEAD
		public Boolean CanIntercept(Object instance)
=======
		public override Boolean CanIntercept(Object instance)
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		{
			return (instance.GetType().GetInterfaces().Length != 0);
		}
	}
}
