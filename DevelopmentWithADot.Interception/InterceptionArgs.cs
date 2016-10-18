using System;
using System.Reflection;

namespace DevelopmentWithADot.Interception
{
	[Serializable]
	public sealed class InterceptionArgs : EventArgs
	{
		private object result;

		public InterceptionArgs(object instance, MethodInfo method, params object[] arguments)
		{
			this.Instance = instance;
			this.Method = method;
			this.Arguments = arguments;
		}

		public void Proceed()
		{
			this.Result = this.Method.Invoke(this.Instance, this.Arguments);
		}

		public object Instance
		{
			get;
			private set;
		}

		public MethodInfo Method
		{
			get;
			private set;
		}

		public object[] Arguments
		{
			get;
			private set;
		}

		public bool Handled
		{
			get;
			set;
		}

		public object Result
		{
			get
			{
				return (this.result);
			}
			set
			{
				this.result = value;
				this.Handled = true;
			}
		}
	}
}
