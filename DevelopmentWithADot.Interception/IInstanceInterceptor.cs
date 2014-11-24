using System;

namespace DevelopmentWithADot.Interception
{
	public interface IInstanceInterceptor : IInterceptor
	{
		Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler);

		Boolean CanIntercept(Object instance);
	}
}
