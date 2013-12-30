using System;

namespace DevelopmentWithADot.Interception
{
	public abstract class InterfaceProxy
	{
		protected readonly Object instance;
		protected readonly Interceptor interceptor;
		protected readonly IInterceptionHandler handler;

		protected InterfaceProxy(Interceptor interceptor, IInterceptionHandler handler, Object instance)
		{
			this.instance = instance;
			this.interceptor = interceptor;
			this.handler = handler;
		}
	}
}
