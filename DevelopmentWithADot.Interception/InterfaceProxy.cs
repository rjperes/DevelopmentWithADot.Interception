using System;

namespace DevelopmentWithADot.Interception
{
	public abstract class InterfaceProxy
	{
		protected readonly Object instance;
<<<<<<< HEAD
		protected readonly IInterceptor interceptor;
		protected readonly IInterceptionHandler handler;

		protected InterfaceProxy(IInterceptor interceptor, IInterceptionHandler handler, Object instance)
=======
		protected readonly Interceptor interceptor;
		protected readonly IInterceptionHandler handler;

		protected InterfaceProxy(Interceptor interceptor, IInterceptionHandler handler, Object instance)
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		{
			this.instance = instance;
			this.interceptor = interceptor;
			this.handler = handler;
		}
	}
}
