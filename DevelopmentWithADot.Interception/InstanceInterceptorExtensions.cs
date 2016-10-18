using System;
using System.Linq;

namespace DevelopmentWithADot.Interception
{
	public static class InstanceInterceptorExtensions
	{
		public static object Intercept(this IInstanceInterceptor interceptor, object instance, IInterceptionHandler handler)
		{
			return (interceptor.Intercept(instance, instance.GetType().GetInterfaces().First(), handler));
		}

		public static T Intercept<T>(this IInstanceInterceptor interceptor, T instance, IInterceptionHandler handler)
		{
			return ((T)interceptor.Intercept((Object)instance, typeof(T), handler));
		}
	}
}
