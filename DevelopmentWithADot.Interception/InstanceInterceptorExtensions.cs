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

<<<<<<< HEAD
		public static T Intercept<T>(this IInstanceInterceptor interceptor, T instance, IInterceptionHandler handler)
=======
		public static T Intercept<T>(this IInstanceInterceptor interceptor, T instance, IInterceptionHandler handler) where T : class
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		{
			return ((T)interceptor.Intercept((Object)instance, typeof(T), handler));
		}
	}
}
