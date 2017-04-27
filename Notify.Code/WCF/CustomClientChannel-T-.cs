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
    public class CustomClientChannel<T> : ChannelFactory<T>
    {
        private readonly string configurationPath;

        private readonly string endpointConfigurationName;

        public CustomClientChannel(string configurationPath)
            : base(typeof(T))
        {
            this.configurationPath = configurationPath;
            this.InitializeEndpoint(configurationName: null, address: null);
        }

        public CustomClientChannel(Binding binding, string configurationPath)
            : this(binding, endpointAddress: null, configurationPath: configurationPath)
        {
        }

        public CustomClientChannel(ServiceEndpoint serviceEndpoint, string configurationPath)
            : base(typeof(T))
        {
            this.configurationPath = configurationPath;
            this.InitializeEndpoint(serviceEndpoint);
        }

        public CustomClientChannel(string endpointConfigurationName, string configurationPath)
            : this(endpointConfigurationName, null, configurationPath)
        {
        }

        public CustomClientChannel(Binding binding, EndpointAddress endpointAddress, string configurationPath)
            : base(typeof(T))
        {
            this.configurationPath = configurationPath;
            this.InitializeEndpoint(binding, endpointAddress);
        }

        public CustomClientChannel(Binding binding, string remoteAddress, string configurationPath)
            : this(binding, new EndpointAddress(remoteAddress), configurationPath)
        {
        }

        public CustomClientChannel(string endpointConfigurationName, EndpointAddress endpointAddress, string configurationPath)
            : base(typeof(T))
        {
            this.configurationPath = configurationPath;
            this.endpointConfigurationName = endpointConfigurationName;
            this.InitializeEndpoint(endpointConfigurationName, endpointAddress);
        }

        protected override ServiceEndpoint CreateDescription()
        {
            ServiceEndpoint serviceEndpoint = base.CreateDescription();
            serviceEndpoint.Name = string.IsNullOrEmpty(this.endpointConfigurationName)
                                       ? string.Empty
                                       : this.endpointConfigurationName;
            Configuration config =
                ConfigurationManager.OpenMappedExeConfiguration(
                    new ExeConfigurationFileMap { ExeConfigFilename = this.configurationPath },
                    ConfigurationUserLevel.None);
            ServiceModelSectionGroup sectionGroup = ServiceModelSectionGroup.GetSectionGroup(config);
            ChannelEndpointElement channelEndpointElement = null;
            if (sectionGroup != null)
            {
                foreach (ChannelEndpointElement channelEndpointElement2 in sectionGroup.Client.Endpoints)
                {
                    if (channelEndpointElement2.Contract == serviceEndpoint.Contract.ConfigurationName
                        && (this.endpointConfigurationName == null
                            || this.endpointConfigurationName == channelEndpointElement2.Name))
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
                        serviceEndpoint.Address = new EndpointAddress(
                            channelEndpointElement.Address,
                            this.GetIdentity(channelEndpointElement.Identity),
                            channelEndpointElement.Headers.Headers);
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

        private void AddBehaviors(string behaviorConfiguration, ServiceEndpoint serviceEndpoint, ServiceModelSectionGroup group)
        {
            if (string.IsNullOrEmpty(behaviorConfiguration))
            {
                return;
            }
            if (group == null)
            {
                return;
            }
            EndpointBehaviorElement endpointBehaviorElement = group.Behaviors.EndpointBehaviors[behaviorConfiguration];
            if (endpointBehaviorElement != null)
            {
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

        private EndpointIdentity GetIdentity(IdentityElement element)
        {
            EndpointIdentity result = null;
            PropertyInformationCollection properties = element.ElementInformation.Properties;
            if (properties == null)
            {
                throw new ArgumentNullException("获取终结点标识属性错误");
            }
            var propertyInformation = properties["userPrincipalName"];
            if (propertyInformation != null && propertyInformation.ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateUpnIdentity(element.UserPrincipalName.Value);
            }
            var information = properties["servicePrincipalName"];
            if (information != null && information.ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateSpnIdentity(element.ServicePrincipalName.Value);
            }
            var propertyInformation1 = properties["dns"];
            if (propertyInformation1 != null && propertyInformation1.ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateDnsIdentity(element.Dns.Value);
            }
            var information1 = properties["rsa"];
            if (information1 != null && information1.ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateRsaIdentity(element.Rsa.Value);
            }
            var propertyInformation2 = properties["certificate"];
            if (propertyInformation2 != null && propertyInformation2.ValueOrigin == PropertyValueOrigin.Default)
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
