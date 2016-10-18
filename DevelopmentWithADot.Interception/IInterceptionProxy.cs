
namespace DevelopmentWithADot.Interception
{
	public interface IInterceptionProxy
	{
		IInterceptor Interceptor
		{
			get;
		}
	}
}
