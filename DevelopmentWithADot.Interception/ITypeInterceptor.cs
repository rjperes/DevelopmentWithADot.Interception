using System;

namespace DevelopmentWithADot.Interception
{
	public interface ITypeInterceptor : IInterceptor
	{
<<<<<<< HEAD
		Type Intercept(Type typeToIntercept, Type interceptionType);
=======
		Type Intercept(Type typeToIntercept, Type handlerType);
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

		Boolean CanIntercept(Type typeToIntercept);
	}
}
