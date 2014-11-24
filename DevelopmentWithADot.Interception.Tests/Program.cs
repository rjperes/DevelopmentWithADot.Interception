using System;

namespace DevelopmentWithADot.Interception.Tests
{
	[InterceptionContext(typeof(MyHandler))]
	class Sample : ContextBoundObject, IMyType
	{
		#region IMyType Members

		public Int32 MyMethod()
		{
			return (0);
		}

		public String MyProperty
		{
			get;
			set;
		}
	}

	#endregion

	public interface IMyType
	{
		Int32 MyMethod();

		String MyProperty
		{
			get;
			set;
		}
	}

	public class MyType : IMyType
	{
		public MyType()
		{

		}

		public virtual String MyProperty
		{
			get
			{
				return (String.Empty);
			}
			set
			{

			}
		}

		public virtual Int32 MyMethod()
		{
			return (10);
		}

		protected virtual void Xpto()
		{

		}
	}

	public class MyHandler : IInterceptionHandler
	{
		#region IInterceptionHandler Members

		public void Invoke(InterceptionArgs arg)
		{
			Console.WriteLine("Returning 20");

			arg.Result = 20;
		}

		#endregion
	}

	class Program
	{
		static T RunTest<T>(IInstanceInterceptor interceptor, T instance) where T : class
		{
			var type = typeof (T);
			var canIntercept = interceptor.CanIntercept(instance);
			var interceptableMethods = interceptor.GetInterceptableMethods(type);
			Console.WriteLine("Can intercept: {0}\tInterceptable methods: {1}", canIntercept, String.Join(", ", interceptableMethods));
			var myProxy = interceptor.Intercept(instance, type, new MyHandler()) as T;
			var proxy = myProxy as IInterceptionProxy;
			if (proxy != null)
			{
				var otherInterceptor = proxy.Interceptor;
			}
			return (myProxy);
		}

		static T RunTest<T>(ITypeInterceptor interceptor) where T : class
		{
			var type = typeof (T);
			var canIntercept = interceptor.CanIntercept(type);
			var interceptableMethods = interceptor.GetInterceptableMethods(type);
			Console.WriteLine("Can intercept: {0}\tInterceptable methods: {1}", canIntercept, String.Join(", ", interceptableMethods));
			var myProxyType = interceptor.Intercept<T>(typeof(MyHandler));
			var myProxy = Activator.CreateInstance(myProxyType) as T;
			var proxy = myProxy as IInterceptionProxy;
			var otherInterceptor = proxy.Interceptor;
			return (myProxy);
		}

		static void Main(String[] args)
		{
			{
				var myProxy = RunTest<MyType>(new VirtualMethodInterceptor());
				var result = myProxy.MyMethod();
			}

			{
				var myProxy = RunTest<IMyType>(new InterfaceInterceptor(), new MyType());
				var result = myProxy.MyMethod();
			}

			{
				var myProxy = RunTest<IMyType>(new TransparentProxyInterceptor(), new MyType());
				var result = myProxy.MyMethod();
			}

			{
				var myProxy = RunTest<IMyType>(new ContextBoundObjectInterceptor(), new Sample());
				var result = myProxy.MyMethod();
			}

			{
				var myProxy = new Sample().Intercept(new MyHandler());
				var result = myProxy.MyMethod();
			}
		}
	}
}
