using System;

namespace DevelopmentWithADot.Interception
{
	public sealed class VirtualMethodInterceptor : ITypeInterceptor
	{
		private readonly InterceptedTypeGenerator generator;

		public VirtualMethodInterceptor(InterceptedTypeGenerator generator)
		{
			this.generator = generator;
		}

		public VirtualMethodInterceptor() : this(CodeDOMInterceptedTypeGenerator.Instance)
		{

		}

		private Type CreateType(Type typeToIntercept, Type handlerType)
		{
			return (this.generator.Generate(this, typeToIntercept, handlerType));
		}

		public Type Intercept(Type typeToIntercept, Type handlerType)
		{
			if (typeToIntercept == null)
			{
				throw (new ArgumentNullException("typeToIntercept"));
			}

			if (handlerType == null)
			{
				throw (new ArgumentNullException("handlerType"));
			}

			if (this.CanIntercept(typeToIntercept) == false)
			{
				throw (new ArgumentException("typeToIntercept"));
			}

			if (typeof(IInterceptionHandler).IsAssignableFrom(handlerType) == false)
			{
				throw (new ArgumentException("handlerType"));
			}

			if (handlerType.IsPublic == false)
			{
				throw (new ArgumentException("handlerType"));
			}

			if ((handlerType.IsAbstract == true) || (handlerType.IsInterface == true))
			{
				throw (new ArgumentException("handlerType"));
			}

			if (handlerType.GetConstructor(Type.EmptyTypes) == null)
			{
				throw (new ArgumentException("handlerType"));
			}

			return (this.CreateType(typeToIntercept, handlerType));
		}

		public bool CanIntercept(Type typeToIntercept)
		{
			return ((typeToIntercept.IsInterface == false) && (typeToIntercept.IsSealed == false));
		}
	}
}
