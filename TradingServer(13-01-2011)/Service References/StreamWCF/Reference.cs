﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradingServer.StreamWCF {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="StreamWCF.IStreamAlertWCF")]
    public interface IStreamAlertWCF {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreamAlertWCF/StreamFile", ReplyAction="http://tempuri.org/IStreamAlertWCF/StreamFileResponse")]
        void StreamFile(string Content);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStreamAlertWCFChannel : TradingServer.StreamWCF.IStreamAlertWCF, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StreamAlertWCFClient : System.ServiceModel.ClientBase<TradingServer.StreamWCF.IStreamAlertWCF>, TradingServer.StreamWCF.IStreamAlertWCF {
        
        public StreamAlertWCFClient() {
        }
        
        public StreamAlertWCFClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StreamAlertWCFClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StreamAlertWCFClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StreamAlertWCFClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void StreamFile(string Content) {
            base.Channel.StreamFile(Content);
        }
    }
}
