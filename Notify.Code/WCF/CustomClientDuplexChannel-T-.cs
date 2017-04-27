using System;
using System.Configuration;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
namespace Notify.Code.WCF
{
	public class CustomClientDuplexChannel<T> : DuplexChannelFactory<T>
	{
		private string configurationPath;
		private string endpointConfigurationName;
		public CustomClientDuplexChannel(object callbackObject, string endpointConfigurationName, string configurationPath) : base(callbackObject, endpointConfigurationName, null)
		{
			this.configurationPath = configurationPath;
			this.endpointConfigurationName = endpointConfigurationName;
			base.InitializeEndpoint(endpointConfigurationName, null);
		}
		public CustomClientDuplexChannel(Type callbackInstanceType, string endpointConfigurationName, string configurationPath) : base(callbackInstanceType, endpointConfigurationName)
		{
			this.configurationPath = configurationPath;
			this.endpointConfigurationName = endpointConfigurationName;
			base.InitializeEndpoint(endpointConfigurationName, null);
		}
		protected override ServiceEndpoint CreateDescription()
		{
			ServiceEndpoint serviceEndpoint = base.CreateDescription();
			if (this.configurationPath != null)
			{
				if (this.endpointConfigurationName != null)
				{
					serviceEndpoint.Name = this.endpointConfigurationName;
				}
				ServiceModelSectionGroup sectionGroup = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
				{
					ExeConfigFilename = this.configurationPath
				}, ConfigurationUserLevel.None));
				ChannelEndpointElement channelEndpointElement = null;
				foreach (ChannelEndpointElement channelEndpointElement2 in sectionGroup.Client.Endpoints)
				{
					if (channelEndpointElement2.Contract == serviceEndpoint.Contract.ConfigurationName && (this.endpointConfigurationName == null || this.endpointConfigurationName == channelEndpointElement2.Name))
					{
						channelEndpointElement = channelEndpointElement2;
						break;
					}
				}
				if (channelEndpointElement != null)
				{
					if (serviceEndpoint.Binding == null)
					{
						serviceEndpoint.Binding = this.CreateBinding(channelEndpointElement.Binding, sectionGroup);
					}
					if (serviceEndpoint.Address == null)
					{
						serviceEndpoint.Address = new EndpointAddress(channelEndpointElement.Address, this.GetIdentity(channelEndpointElement.Identity), channelEndpointElement.Headers.Headers);
					}
					if (serviceEndpoint.Behaviors.Count == 0 && channelEndpointElement.BehaviorConfiguration != null)
					{
						this.AddBehaviors(channelEndpointElement.BehaviorConfiguration, serviceEndpoint, sectionGroup);
					}
					serviceEndpoint.Name = channelEndpointElement.Contract;
				}
			}
			return serviceEndpoint;
		}
		protected override void ApplyConfiguration(string configurationName)
		{
		}
		private void AddBehaviors(string behaviorConfiguration, ServiceEndpoint serviceEndpoint, ServiceModelSectionGroup group)
		{
			if (!string.IsNullOrEmpty(behaviorConfiguration))
			{
				EndpointBehaviorElement endpointBehaviorElement = group.Behaviors.EndpointBehaviors[behaviorConfiguration];
				for (int i = 0; i < endpointBehaviorElement.Count; i++)
				{
					BehaviorExtensionElement behaviorExtensionElement = endpointBehaviorElement[i];
					object obj = behaviorExtensionElement.GetType().InvokeMember("CreateBehavior", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, behaviorExtensionElement, null);
					if (obj != null)
					{
						serviceEndpoint.Behaviors.Add((IEndpointBehavior)obj);
					}
				}
			}
		}
		private Binding CreateBinding(string bindingName, ServiceModelSectionGroup group)
		{
			BindingCollectionElement bindingCollectionElement = group.Bindings[bindingName];
			if (bindingCollectionElement.ConfiguredBindings.Count > 0)
			{
				IBindingConfigurationElement bindingConfigurationElement = bindingCollectionElement.ConfiguredBindings[0];
				Binding binding = this.GetBinding(bindingConfigurationElement);
				if (bindingConfigurationElement != null)
				{
					bindingConfigurationElement.ApplyConfiguration(binding);
				}
				return binding;
			}
			return null;
		}
		private Binding GetBinding(IBindingConfigurationElement configurationElement)
		{
			if (configurationElement is CustomBindingElement)
			{
				return new CustomBinding();
			}
			if (configurationElement is BasicHttpBindingElement)
			{
				return new BasicHttpBinding();
			}
			if (configurationElement is NetMsmqBindingElement)
			{
				return new NetMsmqBinding();
			}
			if (configurationElement is NetNamedPipeBindingElement)
			{
				return new NetNamedPipeBinding();
			}
			if (configurationElement is NetPeerTcpBindingElement)
			{
				return new NetPeerTcpBinding();
			}
			if (configurationElement is NetTcpBindingElement)
			{
				return new NetTcpBinding();
			}
			if (configurationElement is WSDualHttpBindingElement)
			{
				return new WSDualHttpBinding();
			}
			if (configurationElement is WSHttpBindingElement)
			{
				return new WSHttpBinding();
			}
			if (configurationElement is WSFederationHttpBindingElement)
			{
				return new WSFederationHttpBinding();
			}
			return null;
		}
		private EndpointIdentity GetIdentity(IdentityElement element)
		{
			PropertyInformationCollection properties = element.ElementInformation.Properties;
			if (properties["userPrincipalName"].ValueOrigin != PropertyValueOrigin.Default)
			{
				return EndpointIdentity.CreateUpnIdentity(element.UserPrincipalName.Value);
			}
			if (properties["servicePrincipalName"].ValueOrigin != PropertyValueOrigin.Default)
			{
				return EndpointIdentity.CreateSpnIdentity(element.ServicePrincipalName.Value);
			}
			if (properties["dns"].ValueOrigin != PropertyValueOrigin.Default)
			{
				return EndpointIdentity.CreateDnsIdentity(element.Dns.Value);
			}
			if (properties["rsa"].ValueOrigin != PropertyValueOrigin.Default)
			{
				return EndpointIdentity.CreateRsaIdentity(element.Rsa.Value);
			}
			if (properties["certificate"].ValueOrigin == PropertyValueOrigin.Default)
			{
				return null;
			}
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			x509Certificate2Collection.Import(Convert.FromBase64String(element.Certificate.EncodedValue));
			if (x509Certificate2Collection.Count == 0)
			{
				throw new InvalidOperationException("UnableToLoadCertificateIdentity");
			}
			X509Certificate2 primaryCertificate = x509Certificate2Collection[0];
			x509Certificate2Collection.RemoveAt(0);
			return EndpointIdentity.CreateX509CertificateIdentity(primaryCertificate, x509Certificate2Collection);
		}
	}
}
