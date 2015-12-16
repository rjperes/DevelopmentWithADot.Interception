
namespace DevelopmentWithADot.Interception
{
	public interface IProxy
	{
		IInterceptor Interceptor
		{
			get;
		}
	}
}
