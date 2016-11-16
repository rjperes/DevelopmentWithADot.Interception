using System;
using System.Threading;

namespace DevelopmentWithADot.Interception.Tests
{
	public sealed class RetriesInterceptionHandler : IInterceptionHandler
	{
		public int Retries { get; set; }
		public TimeSpan Delay { get; set; }

		public void Invoke(InterceptionArgs arg)
		{
			for (var i = 0; i < this.Retries; i++)
			{
				try
				{
					arg.Proceed();
					break;
				}
				catch
				{
					if (i != this.Retries - 1)
					{
						Thread.Sleep(this.Delay);
					}
					else
					{
						throw;
					}
				}
			}
		}
	}
}
