using System;

namespace DevelopmentWithADot.Interception
{
	public abstract class TypeGenerator
	{
		public abstract Type Generate(Interceptor interceptor, Type baseType, Type handlerType, params Type [] additionalInterfaceTypes);
	}
}
