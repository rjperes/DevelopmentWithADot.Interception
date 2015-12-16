
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevelopmentWithADot.Interception
{
	public abstract class Interceptor : IInterceptor
	{
		public abstract IEnumerable<MethodInfo> GetInterceptableMethods(Type type);
	}
}
