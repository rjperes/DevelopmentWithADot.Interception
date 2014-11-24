using System;
using System.Collections.Generic;

namespace DevelopmentWithADot.Interception
{
	public sealed class MultiInterceptionHandler : IInterceptionHandler
	{
		public MultiInterceptionHandler()
		{
			this.Handlers = new List<IInterceptionHandler>();
		}

		public IList<IInterceptionHandler> Handlers
		{
			get;
			private set;
		}

		#region IInterceptionHandler Members

		public void Invoke(InterceptionArgs arg)
		{
			for (var i = 0; i < this.Handlers.Count; ++i)
			{
				this.Handlers[i].Invoke(arg);

				if (arg.Handled == true)
				{
					break;
				}
			}
		}

		#endregion
	}
}
