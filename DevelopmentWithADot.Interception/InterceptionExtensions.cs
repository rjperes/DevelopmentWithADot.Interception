using System;
using System.Linq;

namespace DevelopmentWithADot.Interception
{
	public static class InterceptionExtensions
	{
		private static IInstanceInterceptor[] instanceInterceptors = typeof (IInstanceInterceptor).Assembly.GetExportedTypes().Where(t => (typeof (IInstanceInterceptor).IsAssignableFrom(t) == true) && (t.IsInterface == false) && (t.IsAbstract == false)).Select(t => Activator.CreateInstance(t)).OfType<IInstanceInterceptor>().ToArray();

		public static T Intercept<T, THandler>(this T instance)
			where T : class
			where THandler : IInterceptionHandler, new()
		{
			return (Intercept<T>(instance, new THandler()));
		}

		public static T Intercept<T>(this T instance, IInterceptionHandler handler)
			where T : class
		{
			foreach (var interceptor in instanceInterceptors)
			{
				if (interceptor.CanIntercept(instance) == true)
				{
					var proxy = interceptor.Intercept(instance, handler);

					return (proxy);
				}
			}

			throw (new ArgumentException("Could not find an interceptor for given type."));
		}
	}
}
