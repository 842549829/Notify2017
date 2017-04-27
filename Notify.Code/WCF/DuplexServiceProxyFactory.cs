using System;
namespace Notify.Code.WCF
{
	public class DuplexServiceProxyFactory
	{
		public static T Create<T, TCallBack>(string configurationPath, string endpointName, TCallBack callBack)
		{
			if (string.IsNullOrEmpty(endpointName))
			{
				throw new ArgumentNullException("endpointName");
			}
			return (T)((object)new DuplexServiceRealProxy<T, TCallBack>(configurationPath, endpointName, callBack).GetTransparentProxy());
		}
		public static string GetEndpointAddress<T, TCallBack>(string configurationPath, string endpointName)
		{
			return DuplexChannelFactoryCreator.Create<T>(Activator.CreateInstance(typeof(TCallBack)), configurationPath, endpointName).Endpoint.Address.Uri.ToString();
		}
	}
}
