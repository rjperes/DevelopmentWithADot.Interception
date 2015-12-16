using System;

namespace DevelopmentWithADot.Interception
{
	public interface ITypeInterceptor : IInterceptor
	{
		Type Intercept(Type typeToIntercept, Type interceptionType);

		Boolean CanIntercept(Type typeToIntercept);
	}
}
