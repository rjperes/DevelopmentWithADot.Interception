using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace DevelopmentWithADot.Interception
{
	public sealed class ContextBoundObjectInterceptor : InstanceInterceptor
	{
		internal static readonly IDictionary<Object, IInterceptionHandler> interceptors = new ConcurrentDictionary<Object, IInterceptionHandler>();

		public override Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler)
		{
			if (instance == null)
			{
				throw (new ArgumentNullException("instance"));
			}

			if (typeToIntercept == null)
			{
				throw (new ArgumentNullException("typeToIntercept"));
			}

			if (handler == null)
			{
				throw (new ArgumentNullException("handler"));
			}

			if (this.CanIntercept(instance) == false)
			{
				throw (new ArgumentException("instance"));
			}

			if (typeToIntercept.IsInstanceOfType(instance) == false)
			{
				throw (new ArgumentException("typeToIntercept"));
			}

			interceptors[instance] = handler;

			return (instance);
		}

		public override Boolean CanIntercept(Object instance)
		{
			return ((instance is ContextBoundObject) && (Attribute.IsDefined(instance.GetType(), typeof(InterceptionContextAttribute))));
		}
	}
}
