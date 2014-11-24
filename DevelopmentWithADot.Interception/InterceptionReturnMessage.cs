using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	internal sealed class InterceptionReturnMessage : IMethodReturnMessage
	{
		private readonly InterceptionArgs args;
		private readonly LogicalCallContext context;
		private readonly IDictionary properties;

		public InterceptionReturnMessage(InterceptionArgs args, LogicalCallContext context, IDictionary properties)
		{
			this.properties = properties;
			this.context = context;
			this.args = args;
		}

		#region IMethodReturnMessage Members

		public Exception Exception
		{
			get
			{
				return (this.args.Exception);
			}
		}

		public Object GetOutArg(Int32 argNum)
		{
			return (this.args.Arguments[argNum]);
		}

		public String GetOutArgName(Int32 index)
		{
			return (this.MethodBase.GetParameters().Where(x => x.IsOut == true).ElementAt(index).Name);
		}

		public Int32 OutArgCount
		{
			get
			{
				return(this.MethodBase.GetParameters().Count(x => x.IsOut == true));
			}
		}

		public Object[] OutArgs
		{
			get
			{
				var indices = this.MethodBase.GetParameters().Select((p, i) => i).ToArray();

				return (this.args.Arguments.Where((a, i) => indices.Contains(i)).ToArray());
			}
		}

		public Object ReturnValue
		{
			get
			{
				return (this.args.Result);
			}
		}

		#endregion

		#region IMethodMessage Members

		public Int32 ArgCount
		{
			get
			{
				return (this.args.Arguments.Length);
			}
		}

		public Object[] Args
		{
			get
			{
				return (this.args.Arguments);
			}
		}

		public Object GetArg(Int32 argNum)
		{
			return (this.args.Arguments[argNum]);
		}

		public String GetArgName(Int32 index)
		{
			return (this.args.Method.GetParameters()[index].Name);
		}

		public Boolean HasVarArgs
		{
			get
			{
				return (this.MethodBase.GetParameters().Any(p => p.GetCustomAttributes(typeof (ParamArrayAttribute), false).Any()));
			}
		}

		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return (this.context);
			}
		}

		public MethodBase MethodBase
		{
			get
			{
				return (this.args.Method);
			}
		}

		public String MethodName
		{
			get
			{
				return (this.MethodBase.Name);
			}
		}

		public Object MethodSignature
		{
			get
			{
				return (this.MethodBase.GetParameters().Select(x => x.ParameterType).ToArray());
			}
		}

		public String TypeName
		{
			get
			{
				return (this.args.Instance.GetType().FullName);
			}
		}

		public String Uri
		{
			get
			{
				return (null);
			}
		}

		#endregion

		#region IMessage Members

		public IDictionary Properties
		{
			get
			{
				return (this.properties);
			}
		}

		#endregion
	}
}
