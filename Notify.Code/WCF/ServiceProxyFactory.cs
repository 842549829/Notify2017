using System;

namespace Notify.Code.WCF
{
	public static class ServiceProxyFactory
	{
		public static T Create<T>(string configurationPath, string endpointName)
		{
            if (string.IsNullOrEmpty(configurationPath))
		    {
                throw new ArgumentNullException("configurationPath");
		    }
			if (string.IsNullOrEmpty(endpointName))
			{
				throw new ArgumentNullException("endpointName");
			}
		    var serviceRealProxy = new ServiceRealProxy<T>(configurationPath, endpointName);
		    return (T)serviceRealProxy.GetTransparentProxy();
			////return (T)new ServiceRealProxy<T>(configurationPath, endpointName).GetTransparentProxy();
		}

		public static string GetEndpointAddress<T>(string configurationPath, string endpointName)
		{
			return ChannelFactoryCreator.Create<T>(configurationPath, endpointName).Endpoint.Address.Uri.ToString();
		}
	}
}
