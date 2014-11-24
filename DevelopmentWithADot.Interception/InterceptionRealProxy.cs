using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace DevelopmentWithADot.Interception
{
	internal sealed class InterceptionRealProxy : RealProxy, IInterceptionProxy
	{
		private readonly Interceptor interceptor;

		public InterceptionRealProxy(Type classToProxy) : base(classToProxy)
		{
		}

		public override IMessage Invoke(IMessage msg)
		{
			var methodName = msg.Properties["__MethodName"] as String;
			var parameterTypes = msg.Properties["__MethodSignature"] as Type[];
			var parameters = msg.Properties["__Args"] as Object[];
			var typeName = msg.Properties["__TypeName"] as String;
			var method = this.GetProxiedType().GetMethod(methodName, parameterTypes);

			throw new NotImplementedException();
		}

		#region IProxy Members

		public Interceptor Interceptor
		{
			get
			{
				return (null);
			}
		}

		#endregion
	}
}
