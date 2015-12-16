using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

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
<<<<<<< HEAD
			for (Int32 i = 0; i < this.Handlers.Count; ++i)
=======
			for (var i = 0; i < this.Handlers.Count; ++i)
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb
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
