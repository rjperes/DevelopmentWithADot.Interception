using System;

namespace DevelopmentWithADot.Interception
{
	public interface IInstanceInterceptor : IInterceptor
	{
		object Intercept(object instance, Type typeToIntercept, IInterceptionHandler handler);

		bool CanIntercept(object instance);
	}
}
