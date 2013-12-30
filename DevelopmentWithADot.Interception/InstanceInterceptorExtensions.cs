using System;
using System.Linq;

namespace DevelopmentWithADot.Interception
{
	public static class InstanceInterceptorExtensions
	{
		public static Object Intercept(this InstanceInterceptor interceptor, Object instance, IInterceptionHandler handler)
		{
			return (interceptor.Intercept(instance, instance.GetType().GetInterfaces().First(), handler));
		}

		public static T Intercept<T>(this InstanceInterceptor interceptor, T instance, IInterceptionHandler handler)
		{
			return ((T)interceptor.Intercept((Object)instance, typeof(T), handler));
		}
	}
}
