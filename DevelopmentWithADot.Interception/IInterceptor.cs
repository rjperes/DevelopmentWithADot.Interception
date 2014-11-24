using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevelopmentWithADot.Interception
{
	public interface IInterceptor
	{
		IEnumerable<MethodInfo> GetInterceptableMethods(Type type);
	}
}
