using System;

namespace DevelopmentWithADot.Interception.Tests
{
<<<<<<< HEAD
    abstract class BaseContextBoundObject : ContextBoundObject, IMyType
    {
        public abstract int MyMethod();

        public abstract string MyProperty { get; set; }
    }

    [InterceptionProxy]
    class Sample1 : BaseContextBoundObject
    {
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

    interface IMyType
    {
        Int32 MyMethod();

        String MyProperty { get; set; }
    }

    class MyType : IMyType
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
            Console.WriteLine("MyType.MyMethod()");
            return (10);
        }
    }

    class MyHandler : IInterceptionHandler
    {
        public void Invoke(InterceptionArgs arg)
        {
            arg.Proceed();
            arg.Result = 20;
            Console.WriteLine("MyHandler.Invoke()");
        }
    }

    class Program
    {
        static void Main(String[] args)
        {
            {
                var interceptor = new DynamicInterceptor();
                var myInstance = new MyType();
                dynamic myProxy = interceptor.Intercept(myInstance, null, new MyHandler());
                var proxy = myProxy as IProxy;
                var otherInterceptor = proxy.Interceptor;
                Int32 result = myProxy.MyMethod();
            }

            {
                var interceptor = new ContextBoundObjectInterceptor();
                var myInstance = new Sample2();
                var myProxy = myInstance = interceptor.Intercept(myInstance, new MyHandler());
                //var proxy = myProxy as IProxy;
                //var otherInterceptor = proxy.Interceptor;
                var result = myProxy.MyMethod();
            }

            {
                var interceptor = new InterfaceInterceptor();
                var myInstance = new MyType();
                var myProxy = interceptor.Intercept(myInstance, typeof(IMyType), new MyHandler()) as IMyType;
                var proxy = myProxy as IProxy;
                var otherInterceptor = proxy.Interceptor;
                var result = myProxy.MyMethod();
            }

            {
                var interceptor = new TransparentProxyInterceptor();
                var myInstance = new MyType();
                var myProxy = interceptor.Intercept(myInstance, new MyHandler()) as IMyType;
                var proxy = myProxy as IProxy;
                var otherInterceptor = proxy.Interceptor;
                var result = myProxy.MyMethod();
            }

            //type interceptor
            {
                var interceptor = new VirtualMethodInterceptor();
                var myProxyType = interceptor.Intercept(typeof(MyType), typeof(MyHandler));
                var myProxy = Activator.CreateInstance(myProxyType) as IMyType;
                var proxy = myProxy as IProxy;
                var otherInterceptor = proxy.Interceptor;
                var result = myProxy.MyMethod();
            }
        }
    }
=======
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
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
}
