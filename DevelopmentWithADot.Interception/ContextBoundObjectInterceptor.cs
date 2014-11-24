using System;
using System.Collections.Concurrent;
using System.Linq;

namespace DevelopmentWithADot.Interception
{
	public sealed class ContextBoundObjectInterceptor : InstanceInterceptor, ICancellableInstanceInterceptor
	{
		internal static readonly ConcurrentDictionary<Object, IInterceptionHandler> interceptors = new ConcurrentDictionary<Object, IInterceptionHandler>();

		public static Boolean IsIntercepted(Object instance)
		{
			return (interceptors.Any(x => x.Key == instance));
		}

		public void StopIntercepting(Object instance)
		{
			var handler = null as IInterceptionHandler;

			foreach (var key in interceptors.Keys)
			{
				if (key == instance)
				{
					interceptors.TryRemove(key, out handler);
					break;
				}
			}
		}

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