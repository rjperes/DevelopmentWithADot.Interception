using System;
using System.Collections.Concurrent;

namespace DevelopmentWithADot.Interception
{
	public sealed class ContextBoundObjectInterceptor : IInstanceInterceptor
	{
		public static readonly IInstanceInterceptor Instance = new ContextBoundObjectInterceptor();

		internal static readonly ConcurrentDictionary<ContextBoundObject, IInterceptionHandler> interceptors = new ConcurrentDictionary<ContextBoundObject, IInterceptionHandler>();

		public object Intercept(object instance, Type typeToIntercept, IInterceptionHandler handler)
		{
			if (!(instance is ContextBoundObject))
			{
				throw new ArgumentException("Instance is not a ContextBoundObject.", "instance");
			}

			interceptors[instance as ContextBoundObject] = handler;

			return (instance);
		}

		public bool CanIntercept(object instance)
		{
			return ((instance is ContextBoundObject) && (Attribute.IsDefined(instance.GetType(), typeof(InterceptionContextAttribute))));
		}

		public static IInterceptionHandler GetInterceptor(ContextBoundObject instance)
		{
			foreach (var @int in interceptors)
			{
			    if (@int.Key == instance)
			    {
			        return @int.Value;
			    }
			}

			return null;
		}
	}
}
