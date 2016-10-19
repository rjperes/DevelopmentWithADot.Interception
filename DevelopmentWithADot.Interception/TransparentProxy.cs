using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace DevelopmentWithADot.Interception
{
	internal sealed class TransparentProxy : RealProxy, IRemotingTypeInfo, IInterceptionProxy
	{
		private readonly object instance;
		private readonly IInterceptionHandler handler;
		private readonly TransparentProxyInterceptor interceptor;

		public TransparentProxy(TransparentProxyInterceptor interceptor, object instance, Type typeToIntercept, IInterceptionHandler handler) : base(typeToIntercept)
		{
			this.instance = instance;
			this.handler = handler;
			this.interceptor = interceptor;
		}

		public override IMessage Invoke(IMessage msg)
		{
			ReturnMessage responseMessage;
			object response = null;
			Exception caughtException = null;

			try
			{
				var methodName = msg.Properties["__MethodName"] as string;
				var parameterTypes = msg.Properties["__MethodSignature"] as Type[];
				var parameters = msg.Properties["__Args"] as object[];
				var typeName = msg.Properties["__TypeName"] as string;
				var method = this.instance.GetType().GetMethod(methodName, parameterTypes);

				if (method == null)
				{
					if (methodName.StartsWith("get_") == true)
					{
						var property = this.instance.GetType().GetProperty(methodName.Substring(4), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

						if (property != null)
						{
							method = property.GetGetMethod();
						}
						else
						{
							if ((methodName == "get_Interceptor") && (typeName == typeof(IInterceptionProxy).AssemblyQualifiedName))
							{
								return (new ReturnMessage(this.interceptor, null, 0, null, msg as IMethodCallMessage));
							}
						}
					}
				}

				var args = new InterceptionArgs(this.instance, method, parameters);

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

			var message = msg as IMethodCallMessage;

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

		bool IRemotingTypeInfo.CanCastTo(Type fromType, object o)
		{
			return (fromType == typeof(IInterceptionProxy));
		}

		string IRemotingTypeInfo.TypeName
		{
			get;
			set;
		}

		#endregion

	    public IInterceptor Interceptor
	    {
	        get { return this.interceptor; }
	    }
	}
}
