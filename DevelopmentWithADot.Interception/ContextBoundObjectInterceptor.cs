using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DevelopmentWithADot.Interception
{
	public class ContextBoundObjectInterceptor : InstanceInterceptor
	{
		internal static IDictionary<Object, IInterceptionHandler> interceptors = new ConcurrentDictionary<Object, IInterceptionHandler>();

		public override Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler)
		{
			interceptors[instance] = handler;

			return (instance);
		}

		public override Boolean CanIntercept(Object instance)
		{
			return ((instance is ContextBoundObject) && ((Attribute.IsDefined(instance.GetType(), typeof(InterceptionProxyAttribute))) || (Attribute.IsDefined(instance.GetType(), typeof(InterceptionContextAttribute)))));
		}
	}
}
