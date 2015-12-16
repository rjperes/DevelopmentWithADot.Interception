using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevelopmentWithADot.Interception
{
	public abstract class InstanceInterceptor : Interceptor, IInstanceInterceptor
	{
		#region Interceptor Members

		public override IEnumerable<MethodInfo> GetInterceptableMethods(Type type)
		{
			if (type.IsInterface == true)
			{
				return (type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
			}
			else
			{
				return (type.GetInterfaces().SelectMany(x => x.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)));
			}
		}

		#endregion

		#region IInstanceInterceptor Members

		public abstract Object Intercept(Object instance, Type typeToIntercept, IInterceptionHandler handler);

		public abstract Boolean CanIntercept(Object instance);

		#endregion
	}
}
