using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;

namespace Notify.Code.WCF
{
	internal class ServiceRealProxy<T> : RealProxy
	{
		private readonly string configurationPath;
		private readonly string endpointName;

		public ServiceRealProxy(string configurationPath, string endpointName) : base(typeof(T))
		{
            if (string.IsNullOrEmpty(configurationPath))
            {
                throw new ArgumentNullException("configurationPath");
            }
			if (string.IsNullOrEmpty(endpointName))
			{
				throw new ArgumentNullException("endpointName");
			}
			this.endpointName = endpointName;
			this.configurationPath = configurationPath;
		}

		public override IMessage Invoke(IMessage msg)
		{
            T t = ChannelFactoryCreator.Create<T>(this.configurationPath, this.endpointName).CreateChannel();
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			IMethodReturnMessage methodReturnMessage;
			object[] array = new object[methodCallMessage.Args.Length];
			methodCallMessage.Args.CopyTo(array, 0);
            //ServiceProxyFactory.GetEndpointAddress<T>(this.configurationPath, this.endpointName);
			try
			{
				object ret = methodCallMessage.MethodBase.Invoke(t, array);
				int outArgsCount = array.Length;
				methodReturnMessage = new ReturnMessage(ret, array, outArgsCount, methodCallMessage.LogicalCallContext, methodCallMessage);
			    var communicationObject = t as ICommunicationObject;
			    if (communicationObject != null)
			    {
			        communicationObject.Close();
			    }
			}
			catch (CommunicationException e)
			{
			    var communicationObject = t as ICommunicationObject;
			    if (communicationObject != null)
			    {
			        communicationObject.Abort();
			    }
			    methodReturnMessage = new ReturnMessage(e, methodCallMessage);
			}
			catch (TimeoutException e2)
			{
			    var communicationObject = t as ICommunicationObject;
			    if (communicationObject != null)
			    {
			        communicationObject.Abort();
			    }
			    methodReturnMessage = new ReturnMessage(e2, methodCallMessage);
			}
			catch (System.Exception e3)
			{
			    var communicationObject = t as ICommunicationObject;
			    if (communicationObject != null)
			    {
			        communicationObject.Abort();
			    }
			    methodReturnMessage = new ReturnMessage(e3, methodCallMessage);
			}
		    return methodReturnMessage;
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
