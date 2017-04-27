using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
namespace Notify.Code.WCF
{
	internal class DuplexServiceRealProxy<T, TCallBack> : RealProxy
	{
		private string configurationPath;
		private string endpointName;
		private TCallBack callBack;
		public DuplexServiceRealProxy(string configurationPath, string endpointName, TCallBack callBack) : base(typeof(T))
		{
			if (string.IsNullOrEmpty(endpointName))
			{
				throw new ArgumentNullException("endpointName");
			}
			this.endpointName = endpointName;
			this.configurationPath = configurationPath;
			this.callBack = callBack;
		}
		public override IMessage Invoke(IMessage msg)
		{
			T t;
			if (this.callBack != null)
			{
				t = DuplexChannelFactoryCreator.Create<T>(this.callBack, this.configurationPath, this.endpointName).CreateChannel();
			}
			else
			{
				t = DuplexChannelFactoryCreator.Create<T>(Activator.CreateInstance(typeof(TCallBack)), this.configurationPath, this.endpointName).CreateChannel();
			}
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			IMethodReturnMessage result = null;
			object[] array = Array.CreateInstance(typeof(object), methodCallMessage.Args.Length) as object[];
			methodCallMessage.Args.CopyTo(array, 0);
			this.GetParameters(methodCallMessage);
			ServiceProxyFactory.GetEndpointAddress<T>(this.configurationPath, this.endpointName);
			try
			{
				result = new ReturnMessage(methodCallMessage.MethodBase.Invoke(t, array), array, array.Length, methodCallMessage.LogicalCallContext, methodCallMessage);
			}
			catch (CommunicationException e)
			{
				(t as ICommunicationObject).Abort();
				result = new ReturnMessage(e, methodCallMessage);
			}
			catch (TimeoutException e2)
			{
				(t as ICommunicationObject).Abort();
				result = new ReturnMessage(e2, methodCallMessage);
			}
			catch (System.Exception e3)
			{
				(t as ICommunicationObject).Abort();
				result = new ReturnMessage(e3, methodCallMessage);
			}
			return result;
		}
		private Dictionary<string, object> GetParameters(IMethodCallMessage mcm)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			for (int i = 0; i < mcm.InArgCount; i++)
			{
				dictionary.Add(mcm.GetInArgName(i), mcm.GetInArg(i));
			}
			return dictionary;
		}
	}
}
