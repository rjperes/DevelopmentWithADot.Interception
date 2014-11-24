using System;
using System.Reflection;

namespace DevelopmentWithADot.Interception
{
	[Serializable]
	public sealed class InterceptionArgs : EventArgs
	{
		private Object result;

		public InterceptionArgs(Object instance, MethodInfo method, params Object [] arguments)
		{
			this.Instance = instance;
			this.Method = method;
			this.Arguments = arguments;
		}

		public void Proceed()
		{
			try
			{
				this.Result = this.Method.Invoke(this.Instance, this.Arguments);
			}
			catch (Exception ex)
			{
				this.Exception = ex;
			}
		}

		public Object Instance
		{
			get;
			private set;
		}

		public MethodInfo Method
		{
			get;
			private set;
		}

		public Object [] Arguments
		{
			get;
			private set;
		}

		public Boolean Handled
		{
			get;
			set;
		}

		public Object Result
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

		public Exception Exception
		{
			get;
			private set;
		}
	}
}
