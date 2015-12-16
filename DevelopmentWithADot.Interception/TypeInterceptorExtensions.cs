﻿using System;

namespace DevelopmentWithADot.Interception
{
	public static class TypeInterceptorExtensions
	{
<<<<<<< HEAD
		public static Type Intercept<TToIntercept, TInterceptor>(this ITypeInterceptor interceptor) where TInterceptor : IInterceptionHandler, new()
		{
			return (interceptor.Intercept(typeof(TToIntercept), typeof(TInterceptor)));
		}
=======
		public static Type Intercept<TToIntercept, TInterceptor>(this ITypeInterceptor interceptor) where TInterceptor : IInterceptionHandler, new() where TToIntercept : class
		{
			return (interceptor.Intercept(typeof(TToIntercept), typeof(TInterceptor)));
		}

		public static Type Intercept<T>(this ITypeInterceptor interceptor, Type handlerType) where T : class
		{
			return (interceptor.Intercept(typeof(T), handlerType));
		}
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
	}
}
