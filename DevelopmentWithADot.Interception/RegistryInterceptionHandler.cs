﻿using System;
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

		public RegistryInterceptionHandler RegisterWhen<T>(Expression<Func<T, object>> method, Func<InterceptionArgs, bool> condition, IInterceptionHandler handler)
		{
			return this.Register<T>(method, new ConditionalInterceptionHandler(condition, handler));
		}

		private RegistryInterceptionHandler RegisterExpressionMethod(Expression expression, IInterceptionHandler handler)
		{
			if (expression is MethodCallExpression)
			{
				return this.Register((expression as MethodCallExpression).Method, handler);
			}
			else if (expression is UnaryExpression)
			{
				if ((expression as UnaryExpression).Operand is MethodCallExpression)
				{
					return this.Register(((expression as UnaryExpression).Operand as MethodCallExpression).Method, handler);
				}
			}
			else if (expression is MemberExpression)
			{
				if ((expression as MemberExpression).Member is PropertyInfo)
				{
					return this
						.Register(((expression as MemberExpression).Member as PropertyInfo).GetMethod, handler)
						.Register(((expression as MemberExpression).Member as PropertyInfo).SetMethod, handler);
				}
			}

			throw new ArgumentException("Expression is not a method call", "method");
		}

		public RegistryInterceptionHandler Register<T>(Expression<Func<T, object>> method, IInterceptionHandler handler)
		{
			return this.RegisterExpressionMethod(method.Body, handler);
		}

		public RegistryInterceptionHandler Register<T>(Expression<Action<T>> method, IInterceptionHandler handler)
		{
			return this.RegisterExpressionMethod(method.Body, handler);
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
