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
		public override RealProxy CreateProxy(ObjRef objRef, Type serverType, Object serverObject, Context serverContext)
		{
			RealProxy proxy = new InterceptionRealProxy(serverType);

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
