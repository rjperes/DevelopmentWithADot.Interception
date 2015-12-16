using System;

namespace DevelopmentWithADot.Interception
{
    public sealed class DynamicInterceptor : IInstanceInterceptor
    {
        public object Intercept(object instance, Type typeToIntercept, IInterceptionHandler handler)
        {
            return new DynamicProxy(this, instance, handler);
        }

        public bool CanIntercept(object instance)
        {
            return true;
        }
    }
}
