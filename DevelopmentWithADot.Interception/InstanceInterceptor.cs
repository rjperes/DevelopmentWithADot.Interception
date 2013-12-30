using System;

namespace DevelopmentWithADot.Interception
{
	public abstract class InstanceInterceptor : Interceptor
	{
		public abstract Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler);
		
		public abstract Boolean CanIntercept(Object instance);		
	}
}
