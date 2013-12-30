using System;

namespace DevelopmentWithADot.Interception
{
	public abstract class InterceptedTypeGenerator
	{
		public abstract Type Generate(Interceptor interceptor, Type baseType, Type handlerType, params Type [] additionalInterfaceTypes);
	}
}
