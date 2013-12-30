using System;

namespace DevelopmentWithADot.Interception
{
	public abstract class TypeInterceptor : Interceptor
	{
		public abstract Type Intercept(Type typeToIntercept, Type interceptionType);

		public abstract Boolean CanIntercept(Type typeToIntercept);		
	}
}
