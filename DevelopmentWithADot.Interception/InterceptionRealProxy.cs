using System;
<<<<<<< HEAD
using System.Reflection;
=======
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace DevelopmentWithADot.Interception
{
<<<<<<< HEAD
	internal sealed class InterceptionRealProxy : RealProxy
	{
=======
	internal sealed class InterceptionRealProxy : RealProxy, IInterceptionProxy
	{
		private readonly Interceptor interceptor;

>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		public InterceptionRealProxy(Type classToProxy) : base(classToProxy)
		{
		}

		public override IMessage Invoke(IMessage msg)
		{
<<<<<<< HEAD
			String methodName = msg.Properties["__MethodName"] as String;
			Type[] parameterTypes = msg.Properties["__MethodSignature"] as Type[];
			Object[] parameters = msg.Properties["__Args"] as Object[];
			String typeName = msg.Properties["__TypeName"] as String;
			MethodInfo method = this.GetProxiedType().GetMethod(methodName, parameterTypes);

			throw new NotImplementedException();
		}
=======
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
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
	}
}
