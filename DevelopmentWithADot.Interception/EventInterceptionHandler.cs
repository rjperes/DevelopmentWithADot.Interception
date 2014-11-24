using System;

namespace DevelopmentWithADot.Interception
{
	public sealed class EventInterceptionHandler : IInterceptionHandler
	{
		public event EventHandler<InterceptionArgs> Interception;

		#region IInterceptionHandler Members

		public void Invoke(InterceptionArgs arg)
		{
			var handler = this.Interception;

			if (handler != null)
			{
				handler(this, arg);
			}
		}

		#endregion
	}
}
