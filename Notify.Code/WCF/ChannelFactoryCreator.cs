using System;
using System.Collections;
using System.ServiceModel;

namespace Notify.Code.WCF
{
	internal static class ChannelFactoryCreator
	{
		private static readonly Hashtable channelFactories = new Hashtable();

		public static ChannelFactory<T> Create<T>(string configurationPath, string endpointName)
		{
			if (string.IsNullOrEmpty(endpointName))
			{
				throw new ArgumentNullException("endpointName");
			}
			ChannelFactory<T> channelFactory = null;
			if (channelFactories.ContainsKey(endpointName))
			{
				channelFactory = channelFactories[endpointName] as ChannelFactory<T>;
			}
			if (channelFactory == null)
			{
				channelFactory = new CustomClientChannel<T>(endpointName, configurationPath);
				lock (channelFactories.SyncRoot)
				{
					channelFactories[endpointName] = channelFactory;
				}
			}
			return channelFactory;
		}
	}
}
