using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace DevelopmentWithADot.Interception
{
<<<<<<< HEAD
	internal sealed class TransparentProxy : RealProxy, IRemotingTypeInfo, IProxy
=======
	internal sealed class TransparentProxy : RealProxy, IRemotingTypeInfo
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
	{
		private readonly Object instance;
		private readonly IInterceptionHandler handler;
		private readonly TransparentProxyInterceptor interceptor;

		public TransparentProxy(TransparentProxyInterceptor interceptor, Object instance, Type typeToIntercept, IInterceptionHandler handler) : base(typeToIntercept)
		{
			this.instance = instance;
			this.handler = handler;
			this.interceptor = interceptor;
		}

		public override IMessage Invoke(IMessage msg)
		{
<<<<<<< HEAD
			ReturnMessage responseMessage;
			Object response = null;
			Exception caughtException = null;

			try
			{
				String methodName = msg.Properties["__MethodName"] as String;
				Type[] parameterTypes = msg.Properties["__MethodSignature"] as Type[];
				Object[] parameters = msg.Properties["__Args"] as Object[];
				String typeName = msg.Properties["__TypeName"] as String;
				MethodInfo method = this.instance.GetType().GetMethod(methodName, parameterTypes);
=======
			var responseMessage = null as ReturnMessage;
			var response = null as Object;
			var caughtException = null as Exception;

			try
			{
				var methodName = msg.Properties["__MethodName"] as String;
				var parameterTypes = msg.Properties["__MethodSignature"] as Type[];
				var parameters = msg.Properties["__Args"] as Object[];
				var typeName = msg.Properties["__TypeName"] as String;
				var method = this.instance.GetType().GetMethod(methodName, parameterTypes);
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

				if (method == null)
				{
					if (methodName.StartsWith("get_") == true)
					{
<<<<<<< HEAD
						PropertyInfo property = this.instance.GetType().GetProperty(methodName.Substring(4), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
=======
						var property = this.instance.GetType().GetProperty(methodName.Substring(4), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

						if (property != null)
						{
							method = property.GetGetMethod();
						}
						else
						{
<<<<<<< HEAD
							if ((methodName == "get_Interceptor") && (typeName == typeof(IProxy).AssemblyQualifiedName))
=======
							if ((methodName == "get_Interceptor") && (typeName == typeof(IInterceptionProxy).AssemblyQualifiedName))
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
							{
								return (new ReturnMessage(this.interceptor, null, 0, null, msg as IMethodCallMessage));
							}
						}
					}
				}

<<<<<<< HEAD
				InterceptionArgs args = new InterceptionArgs(this.instance, method, parameters);
=======
				var args = new InterceptionArgs(this.instance, method, parameters);
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

				this.handler.Invoke(args);

				if (args.Handled == false)
				{
					args.Proceed();
				}

				response = args.Result;
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}

<<<<<<< HEAD
			IMethodCallMessage message = msg as IMethodCallMessage;
=======
			var message = msg as IMethodCallMessage;
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

			if (caughtException == null)
			{
				responseMessage = new ReturnMessage(response, null, 0, null, message);
			}
			else
			{
				responseMessage = new ReturnMessage(caughtException, message);
			}

			return (responseMessage);
		}

		#region IRemotingTypeInfo Members

		Boolean IRemotingTypeInfo.CanCastTo(Type fromType, Object o)
		{
<<<<<<< HEAD
			return (fromType == typeof(IProxy));
=======
			return (fromType == typeof(IInterceptionProxy));
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		}

		String IRemotingTypeInfo.TypeName
		{
			get;
			set;
		}

		#endregion
<<<<<<< HEAD

	    public IInterceptor Interceptor
	    {
	        get { return this.interceptor; }
	    }
=======
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
	}
}
