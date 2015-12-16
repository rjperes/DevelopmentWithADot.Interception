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
<<<<<<< HEAD
			this.Result = this.Method.Invoke(this.Instance, this.Arguments);
=======
			try
			{
				this.Result = this.Method.Invoke(this.Instance, this.Arguments);
			}
			catch (Exception ex)
			{
				this.Exception = ex;
			}
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
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
<<<<<<< HEAD
=======

		public Exception Exception
		{
			get;
			private set;
		}
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
	}
}
