using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace DevelopmentWithADot.Interception
{
	internal sealed class TransparentProxy : RealProxy, IRemotingTypeInfo, IProxy
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

				if (method == null)
				{
					if (methodName.StartsWith("get_") == true)
					{
						PropertyInfo property = this.instance.GetType().GetProperty(methodName.Substring(4), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

						if (property != null)
						{
							method = property.GetGetMethod();
						}
						else
						{
							if ((methodName == "get_Interceptor") && (typeName == typeof(IProxy).AssemblyQualifiedName))
							{
								return (new ReturnMessage(this.interceptor, null, 0, null, msg as IMethodCallMessage));
							}
						}
					}
				}

				InterceptionArgs args = new InterceptionArgs(this.instance, method, parameters);

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

			IMethodCallMessage message = msg as IMethodCallMessage;

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
			return (fromType == typeof(IProxy));
		}

		String IRemotingTypeInfo.TypeName
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
