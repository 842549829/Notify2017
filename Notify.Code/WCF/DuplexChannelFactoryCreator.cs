using System;
using System.Collections;
using System.ServiceModel;
namespace Notify.Code.WCF
{
	internal class DuplexChannelFactoryCreator
	{
		private static Hashtable channelFactories = new Hashtable();
		public static DuplexChannelFactory<T> Create<T>(object callbackObject, string configurationPath, string endpointName)
		{
			if (string.IsNullOrEmpty(endpointName))
			{
				throw new ArgumentNullException("endpointName");
			}
			DuplexChannelFactory<T> duplexChannelFactory = null;
			if (DuplexChannelFactoryCreator.channelFactories.ContainsKey(endpointName))
			{
				duplexChannelFactory = (DuplexChannelFactoryCreator.channelFactories[endpointName] as DuplexChannelFactory<T>);
			}
			if (duplexChannelFactory == null)
			{
				duplexChannelFactory = new CustomClientDuplexChannel<T>(callbackObject, endpointName, configurationPath);
				lock (DuplexChannelFactoryCreator.channelFactories.SyncRoot)
				{
					DuplexChannelFactoryCreator.channelFactories[endpointName] = duplexChannelFactory;
				}
			}
			return duplexChannelFactory;
		}
	}
}
