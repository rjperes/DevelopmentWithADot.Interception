using System.Runtime.Remoting.Messaging;

namespace DevelopmentWithADot.Interception
{
	internal sealed class InterceptionMessageSink : IMessageSink
	{
		public InterceptionMessageSink(IMessageSink next)
		{
			this.NextSink = next;
		}

		public IMessageSink NextSink
		{
			get;
			set;
		}

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{

			return (this.NextSink.AsyncProcessMessage(msg, replySink));
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{

			return (this.NextSink.SyncProcessMessage(msg));
		}
	}
}
