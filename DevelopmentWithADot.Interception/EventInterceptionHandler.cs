using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

namespace DevelopmentWithADot.Interception
{
	public sealed class EventInterceptionHandler : IInterceptionHandler
	{
		public event EventHandler<InterceptionArgs> Interception;

		#region IInterceptionHandler Members

		public void Invoke(InterceptionArgs arg)
		{
<<<<<<< HEAD
			EventHandler<InterceptionArgs> handler = this.Interception;
=======
			var handler = this.Interception;
>>>>>>> 59b505f23b739272092e29d693382916e938e4bb

			if (handler != null)
			{
				handler(this, arg);
			}
		}

		#endregion
	}
}
