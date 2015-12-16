using System;

namespace DevelopmentWithADot.Interception
{
	public interface ICancellableInstanceInterceptor : IInstanceInterceptor
	{
		void StopIntercepting(Object instance);
	}
}
