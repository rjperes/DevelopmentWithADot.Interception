using System;
using System.Collections.Concurrent;
<<<<<<< HEAD

namespace DevelopmentWithADot.Interception
{
	public sealed class ContextBoundObjectInterceptor : IInstanceInterceptor
	{
		internal static readonly ConcurrentDictionary<ContextBoundObject, IInterceptionHandler> interceptors = new ConcurrentDictionary<ContextBoundObject, IInterceptionHandler>();

		public Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler)
		{
			if (!(instance is ContextBoundObject))
			{
				throw new ArgumentException("Instance is not a ContextBoundObject.", "instance");
			}

			interceptors[instance as ContextBoundObject] = handler;

			return (instance);
		}

		public Boolean CanIntercept(Object instance)
		{
			return ((instance is ContextBoundObject) && ((Attribute.IsDefined(instance.GetType(), typeof(InterceptionProxyAttribute))) || (Attribute.IsDefined(instance.GetType(), typeof(InterceptionContextAttribute)))));
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
=======
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
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
