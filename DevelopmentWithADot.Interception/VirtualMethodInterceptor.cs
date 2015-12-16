using System;
<<<<<<< HEAD

namespace DevelopmentWithADot.Interception
{
	public sealed class VirtualMethodInterceptor : ITypeInterceptor
	{
		private static readonly InterceptedTypeGenerator generator = new CodeDOMInterceptedTypeGenerator();
=======
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevelopmentWithADot.Interception
{
	public sealed class VirtualMethodInterceptor : Interceptor, ITypeInterceptor
	{
		private static readonly TypeGenerator generator = new CodeDOMTypeGenerator();
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

		private Type CreateType(Type typeToIntercept, Type handlerType)
		{
			return (generator.Generate(this, typeToIntercept, handlerType));
		}

<<<<<<< HEAD
=======
		public override IEnumerable<MethodInfo> GetInterceptableMethods(Type type)
		{
			return (type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => (x.IsAbstract == true) || (x.IsVirtual == true)));
		}

>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
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

		public Boolean CanIntercept(Type typeToIntercept)
		{
			return ((typeToIntercept.IsInterface == false) && (typeToIntercept.IsSealed == false));
		}
	}
}
