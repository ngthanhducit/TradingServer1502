﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradingServer.AgentWCF {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AgentWCF.IDefaultWCF")]
    public interface IDefaultWCF {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDefaultWCF/DefaultPort", ReplyAction="http://tempuri.org/IDefaultWCF/DefaultPortResponse")]
        void DefaultPort(string cmd, string ipAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDefaultWCF/StringDefaultPort", ReplyAction="http://tempuri.org/IDefaultWCF/StringDefaultPortResponse")]
        string StringDefaultPort(string cmd, string ipAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDefaultWCF/ListStringDefaultPort", ReplyAction="http://tempuri.org/IDefaultWCF/ListStringDefaultPortResponse")]
        string[] ListStringDefaultPort(string cmd, string ipAddress);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDefaultWCFChannel : TradingServer.AgentWCF.IDefaultWCF, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DefaultWCFClient : System.ServiceModel.ClientBase<TradingServer.AgentWCF.IDefaultWCF>, TradingServer.AgentWCF.IDefaultWCF {
        
        public DefaultWCFClient() {
        }
        
        public DefaultWCFClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DefaultWCFClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DefaultWCFClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DefaultWCFClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void DefaultPort(string cmd, string ipAddress) {
            base.Channel.DefaultPort(cmd, ipAddress);
        }
        
        public string StringDefaultPort(string cmd, string ipAddress) {
            return base.Channel.StringDefaultPort(cmd, ipAddress);
        }
        
        public string[] ListStringDefaultPort(string cmd, string ipAddress) {
            return base.Channel.ListStringDefaultPort(cmd, ipAddress);
        }
    }
}
