using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Notify.Code.WCF
{
	public static class CommonCommunication
	{
		public static string ServerIP
		{
			get
			{
				return OperationContext.Current.Channel.LocalAddress.Uri.Host;
			}
		}
		public static string ClientIP
		{
			get
			{
				MessageProperties incomingMessageProperties = OperationContext.Current.IncomingMessageProperties;
				if (incomingMessageProperties.Keys.Contains(RemoteEndpointMessageProperty.Name))
				{
					RemoteEndpointMessageProperty remoteEndpointMessageProperty = incomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
					if (remoteEndpointMessageProperty != null)
					{
						return remoteEndpointMessageProperty.Address;
					}
					return "unkown";
				}
				else
				{
					if (OperationContext.Current.Channel.RemoteAddress == null)
					{
						return "unkown";
					}
					return OperationContext.Current.Channel.RemoteAddress.Uri.Host;
				}
			}
		}
	}
}
