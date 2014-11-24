using System;

namespace DevelopmentWithADot.Interception
{
	public interface ITypeInterceptor : IInterceptor
	{
		Type Intercept(Type typeToIntercept, Type handlerType);

		Boolean CanIntercept(Type typeToIntercept);
	}
}
