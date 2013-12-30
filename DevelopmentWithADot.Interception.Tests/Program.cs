using System;

namespace DevelopmentWithADot.Interception.Tests
{
	abstract class BaseContextBoundObject : ContextBoundObject, IMyType
	{
		public abstract int MyMethod();

		public abstract string MyProperty { get; set; }
	}

	[InterceptionProxy]
	class Sample1 : BaseContextBoundObject
	{
		#region IMyType Members

		public override int MyMethod()
		{
			return (0);
		}

		public override string MyProperty
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
		#region IMyType Members

		public override int MyMethod()
		{
			return (0);
		}

		public override string MyProperty
		{
			get
			{
				return ("");
			}
			set
			{
			}
		}

		#endregion
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

		protected MyType(int i)
		{

		}

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

		protected virtual void Xpto()
		{

		}
	}

	public class MyHandler : IInterceptionHandler
	{
		#region IInterceptionHandler Members

		public void Invoke(InterceptionArgs arg)
		{
			arg.Result = 20;
		}

		#endregion
	}

	class Program
	{
		static void Main(String[] args)
		{
			{
				Sample2 s2 = new Sample2();
				s2.MyMethod();

				return;
			}

			{
				InstanceInterceptor interceptor = new InterfaceInterceptor();
				IMyType myInstance = new MyType();
				IMyType myProxy = interceptor.Intercept(myInstance, typeof(IMyType), new MyHandler()) as IMyType;
				IProxy proxy = myProxy as IProxy;
				Interceptor otherInterceptor = proxy.Interceptor;
				Int32 result = myProxy.MyMethod();
			}

			{
				TypeInterceptor interceptor = new VirtualMethodInterceptor();
				Type myProxyType = interceptor.Intercept(typeof(MyType), typeof(MyHandler));
				MyType myProxy = Activator.CreateInstance(myProxyType) as MyType;
				IProxy proxy = myProxy as IProxy;
				Interceptor otherInterceptor = proxy.Interceptor;
				Int32 result = myProxy.MyMethod();
			}

			{
				InstanceInterceptor interceptor = new TransparentProxyInterceptor();
				IMyType myInstance = new MyType();
				IMyType myProxy = interceptor.Intercept(myInstance, new MyHandler());
				IProxy proxy = myProxy as IProxy;
				Interceptor otherInterceptor = proxy.Interceptor;
				Int32 result = myProxy.MyMethod();
			}
		}
	}
}
