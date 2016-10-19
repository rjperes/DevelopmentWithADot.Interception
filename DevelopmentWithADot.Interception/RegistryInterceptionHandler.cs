using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DevelopmentWithADot.Interception
{
	public sealed class RegistryInterceptionHandler : IInterceptionHandler
	{
		private readonly Dictionary<MethodInfo, IInterceptionHandler> handlers = new Dictionary<MethodInfo, IInterceptionHandler>();

		public RegistryInterceptionHandler Register(Type type, string methodName, IInterceptionHandler handler)
		{
			foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
			{
				this.Register(method, handler);
			}

			return this;
		}

		public RegistryInterceptionHandler Register(MethodInfo method, IInterceptionHandler handler)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}

			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}

			this.handlers[method] = handler;

			return this;
		}

		public RegistryInterceptionHandler Register<T>(Expression<Func<T, object>> method, IInterceptionHandler handler)
		{
			if (!(method.Body is MethodCallExpression))
			{
				throw new ArgumentException("Expression is not a method call", "method");
			}

			return this.Register((method.Body as MethodCallExpression).Method, handler);
		}

		public RegistryInterceptionHandler Register<T>(Expression<Action<T>> method, IInterceptionHandler handler)
		{
			if (!(method.Body is MethodCallExpression))
			{
				throw new ArgumentException("Expression is not a method call", "method");
			}

			return this.Register((method.Body as MethodCallExpression).Method, handler);
		}

		public void Invoke(InterceptionArgs arg)
		{
			IInterceptionHandler handler;

			if (this.handlers.TryGetValue(arg.Method, out handler) == true)
			{
				handler.Invoke(arg);
			}
		}
	}
}
