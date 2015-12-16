using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Proxies;

namespace DevelopmentWithADot.Interception
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class InterceptionProxyAttribute : ProxyAttribute
	{
<<<<<<< HEAD
=======
		private readonly Type interceptionHandler;

		public InterceptionProxyAttribute(Type interceptionHandler)
		{
			this.interceptionHandler = interceptionHandler;
		}

>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
		public override RealProxy CreateProxy(ObjRef objRef, Type serverType, Object serverObject, Context serverContext)
		{
			var proxy = new InterceptionRealProxy(serverType);

			if (serverContext != null)
			{
				RealProxy.SetStubData(proxy, serverContext);
			}

			if ((serverType.IsMarshalByRef == false) && (serverContext == null))
			{
				throw new RemotingException("Bad Type for CreateProxy");
			}

			return (proxy);
		}
	}
}
