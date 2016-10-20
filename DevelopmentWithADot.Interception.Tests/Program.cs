using NUnit.Framework;
using System;

namespace DevelopmentWithADot.Interception.Tests
{
	abstract class BaseContextBoundObject : ContextBoundObject, IMyType
	{
		public abstract int MyMethod();

		public abstract string MyProperty { get; set; }
	}

	//[InterceptionProxy]
	/*class Sample1 : BaseContextBoundObject
	{
		public override Int32 MyMethod()
		{
			return (0);
		}

		public override String MyProperty
		{
			get
			{
				return ("");
			}
			set
			{
			}
		}
	}*/

	[InterceptionContext]
	class MyType2 : BaseContextBoundObject
	{
		public override int MyMethod()
		{
			return (0);
		}

		public override String MyProperty { get; set; }
	}

	public interface IMyType
	{
		int MyMethod();

		string MyProperty { get; set; }
	}

	public class MyType : MarshalByRefObject, IMyType
	{
		public virtual string MyProperty { get; set; }

		public virtual int MyMethod()
		{
			return (0);
		}
	}

	public class MyType3 : IMyType
	{
		public virtual string MyProperty { get; set; }

		[ConsoleLogInterception]
		public virtual int MyMethod()
		{
			return (0);
		}
	}

	public class MyHandler : IInterceptionHandler
	{
		public void Invoke(InterceptionArgs arg)
		{
			arg.Result = 20;
		}
	}

	static class Program
	{
		static void DynamicInterceptor(object instance)
		{
			//Dynamic interceptor
			var interceptor = new DynamicInterceptor();
			var canIntercept = interceptor.CanIntercept(instance);
			dynamic myProxy = interceptor.Intercept(instance, null, new MyHandler());
			var proxy = myProxy as IInterceptionProxy;
			var otherInterceptor = proxy.Interceptor;
			int result = myProxy.MyMethod();
			Assert.AreEqual(20, result);
		}

		static void ContextBoundObjectInterceptor(ContextBoundObject instance)
		{
			//Context bound object interceptor
			var interceptor = new ContextBoundObjectInterceptor();
			var canIntercept = interceptor.CanIntercept(instance);
			var myProxy = interceptor.Intercept(instance, new MyHandler());
			//var proxy = myProxy as IInterceptionProxy;
			//var otherInterceptor = proxy.Interceptor;
			var result = (myProxy as IMyType).MyMethod();
			Assert.AreEqual(20, result);
		}

		static void InterfaceInterceptor(object instance)
		{
			//Interface interceptor
			var interceptor = new InterfaceInterceptor();
			var canIntercept = interceptor.CanIntercept(instance);
			var myProxy = interceptor.Intercept(instance, typeof(IMyType), new MyHandler()) as IMyType;
			var proxy = myProxy as IInterceptionProxy;
			var otherInterceptor = proxy.Interceptor;
			var result = myProxy.MyMethod();
			Assert.AreEqual(20, result);
		}

		static void VirtualMethodInterceptor(Type type)
		{
			//Virtual method interceptor
			var interceptor = new VirtualMethodInterceptor();
			var canIntercept = interceptor.CanIntercept(type);
			var myProxyType = interceptor.Intercept(type, typeof(MyHandler));
			//var myProxyType = interceptor.Intercept<MyType, MyHandler>();
			var myProxy = Activator.CreateInstance(myProxyType) as IMyType;
			var proxy = myProxy as IInterceptionProxy;
			var otherInterceptor = proxy.Interceptor;
			var result = myProxy.MyMethod();
			Assert.AreEqual(20, result);
		}

		static void TransparentProxyInterceptor(MarshalByRefObject instance)
		{
			//Transparent proxy interceptor
			var interceptor = new TransparentProxyInterceptor();
			var canIntercept = interceptor.CanIntercept(instance);
			var myProxy = interceptor.Intercept(instance, typeof(IMyType), new MyHandler()) as IMyType;
			var proxy = myProxy as IInterceptionProxy;
			var otherInterceptor = proxy.Interceptor;
			var result = myProxy.MyMethod();
			Assert.AreEqual(20, result);
		}

		static void DynamicInterceptorWithAttributes(object instance)
		{
			dynamic myProxy = DevelopmentWithADot.Interception.DynamicInterceptor.Instance.InterceptWithAttributes(instance);
			myProxy.MyMethod();
		}

		static void DynamicInterceptorWithRegistry(object instance)
		{
			var interceptor = new DynamicInterceptor();
			var registry = new RegistryInterceptionHandler();
			registry.Register<IMyType>(x => x.MyMethod(), new MyHandler());
			dynamic myProxy = interceptor.Intercept(instance, null, registry);
			myProxy.MyMethod();
		}

		static void Main()
		{
			DynamicInterceptor(new MyType());
			ContextBoundObjectInterceptor(new MyType2());
			InterfaceInterceptor(new MyType());
			VirtualMethodInterceptor(typeof(MyType));
			TransparentProxyInterceptor(new MyType());

			DynamicInterceptorWithAttributes(new MyType3());
			DynamicInterceptorWithRegistry(new MyType3());
		}
	}
}
