using System;

namespace DevelopmentWithADot.Interception.Tests
{
	abstract class BaseContextBoundObject : ContextBoundObject, IMyType
	{
		public abstract Int32 MyMethod();

		public abstract String MyProperty { get; set; }
	}

	//[InterceptionProxy]
	class Sample1 : BaseContextBoundObject
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
	}

	[InterceptionContext]
	class Sample2 : BaseContextBoundObject
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
	}

	public interface IMyType
	{
		Int32 MyMethod();

		String MyProperty { get; set; }
	}

	public class MyType : MarshalByRefObject, IMyType
	{
		public virtual String MyProperty
		{
			get
			{
				return ("");
			}
			set
			{

			}
		}

		public virtual Int32 MyMethod()
		{
			return (10);
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
		static void Main()
		{
			{
				var interceptor = new DynamicInterceptor();
				var myInstance = new MyType();
				dynamic myProxy = interceptor.Intercept(myInstance, null, new MyHandler());
				var proxy = myProxy as IInterceptionProxy;
				var otherInterceptor = proxy.Interceptor;
				Int32 result = myProxy.MyMethod();
			}

			{
				var interceptor = new ContextBoundObjectInterceptor();
				var myInstance = new Sample2();
				var myProxy = myInstance = interceptor.Intercept(myInstance, new MyHandler());
				//var proxy = myProxy as IInterceptionProxy;
				//var otherInterceptor = proxy.Interceptor;
				var result = myProxy.MyMethod();
			}

			{
				var interceptor = new InterfaceInterceptor();
				var myInstance = new MyType();
				var myProxy = interceptor.Intercept(myInstance, typeof(IMyType), new MyHandler()) as IMyType;
				var proxy = myProxy as IInterceptionProxy;
				var otherInterceptor = proxy.Interceptor;
				var result = myProxy.MyMethod();
			}

			{
				var interceptor = new VirtualMethodInterceptor();
				var myProxyType = interceptor.Intercept(typeof(MyType), typeof(MyHandler));
				//var myProxyType = interceptor.Intercept<MyType, MyHandler>();
				var myProxy = Activator.CreateInstance(myProxyType) as IMyType;
				var proxy = myProxy as IInterceptionProxy;
				var otherInterceptor = proxy.Interceptor;
				var result = myProxy.MyMethod();
			}

			{
				var interceptor = new TransparentProxyInterceptor();
				var myInstance = new MyType();
				var myProxy = interceptor.Intercept(myInstance, new MyHandler()) as IMyType;
				var proxy = myProxy as IInterceptionProxy;
				var otherInterceptor = proxy.Interceptor;
				var result = myProxy.MyMethod();
			}
		}
	}
}
