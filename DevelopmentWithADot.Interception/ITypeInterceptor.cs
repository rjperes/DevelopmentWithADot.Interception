using System;

namespace DevelopmentWithADot.Interception
{
	public interface ITypeInterceptor : IInterceptor
	{
		Type Intercept(Type typeToIntercept, Type interceptionType);

		bool CanIntercept(Type typeToIntercept);
	}
}
