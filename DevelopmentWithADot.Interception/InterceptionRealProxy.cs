using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace DevelopmentWithADot.Interception
{
	internal sealed class InterceptionRealProxy : RealProxy
	{
		public InterceptionRealProxy(Type classToProxy) : base(classToProxy)
		{
		}

		public override IMessage Invoke(IMessage msg)
		{
			String methodName = msg.Properties["__MethodName"] as String;
			Type[] parameterTypes = msg.Properties["__MethodSignature"] as Type[];
			Object[] parameters = msg.Properties["__Args"] as Object[];
			String typeName = msg.Properties["__TypeName"] as String;
			MethodInfo method = this.GetProxiedType().GetMethod(methodName, parameterTypes);

			throw new NotImplementedException();
		}
	}
}
