using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			for (Int32 i = 0; i < this.Handlers.Count; ++i)
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
