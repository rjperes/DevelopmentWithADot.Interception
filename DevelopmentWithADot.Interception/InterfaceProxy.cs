namespace DevelopmentWithADot.Interception
{
	public abstract class InterfaceProxy
	{
		protected readonly object instance;
		protected readonly IInterceptor interceptor;
		protected readonly IInterceptionHandler handler;

		protected InterfaceProxy(IInterceptor interceptor, IInterceptionHandler handler, object instance)
		{
			this.instance = instance;
			this.interceptor = interceptor;
			this.handler = handler;
		}
	}
}
