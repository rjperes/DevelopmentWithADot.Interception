using System;
using System.Linq;

namespace DevelopmentWithADot.Interception
{
	public static class InstanceInterceptorExtensions
	{
		public static Object Intercept(this IInstanceInterceptor interceptor, Object instance, IInterceptionHandler handler)
		{
			return (interceptor.Intercept(instance, instance.GetType().GetInterfaces().First(), handler));
		}

		public static T Intercept<T>(this IInstanceInterceptor interceptor, T instance, IInterceptionHandler handler)
		{
			return ((T)interceptor.Intercept((Object)instance, typeof(T), handler));
		}
	}
}
