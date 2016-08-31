using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
//WCF is a webservice - a web service is like a website that is used by a computer instead of a person
//For ex, instead of browsing to a website about your fav TV program, you might instead use a desktop 
//application that pulled the same information via a web service
//.Net => web services have been supported, however in the most recent versions of the framework,
//web services have been combined with another technology called remoting to create 
//WCF(windows communication foundation), which is a generic structure for communication bet applications
//Remoting allows you to create instances of objects in one process and use them from another process
//-even if the object is created on a computer other than the one that is using it
//WCF takes concepts such as services and platform-independent SOAP messages from web services,
//and combines these with concepts such as host server applications and advanced binding capabilities
//from remoting. Result is tech you can think of as a superset that includes both web services and remoting,
//but that is more powerful than web services and much easier to use than remoting. Using WCF, you can 
//make applications that use a service orientated architechture(SOA), which means that you decentralize
//processing and make use of distributed processing by connecting to services and data as you need them across 
//local networks and the internet. 

//WCF is a technology that enables you to create services that you can access from other applications across
//process, machine, and network boundaries. You can use theses services to share functionality across multiple
//applications, to expose data sources, or to abstract complicated processes. 
//The functioality of WCF services offer is encapsulated as individual methods that are exposed
//by the service. Each method - or operation in WCF terms, has an endpoint that you exchange data with in order
//to use it.  This data exchange can be defined by one or more protocols, depending on the network that 
//you use to connect to the service and your specific requirements.
//In WCF, an endpoint can have multiple bindings, each of which specifies a means of communicaion. 
//Binding can also specify additional information such as which security requirements must be 
//met to communicate with the endpoint. => might req a usernae and pass, or Windows user acnt token, . 
//When you connect to an endpoint, the protocol that the binding uses affects the address that you use.
//Once you have connected to an endpoint, you can communicate with it by using Simple Object Access Protocol(SOAP)
//messages. The form of the messagees that you use depends on the operation you are using and the data structures
//that are required to send messages to(and recieve messages from) that operation. WCF uses contracts to specify
//all this. You can discover contracts through metadata(a set of data that describes and gives information 
//about other data.) exchange with a service. WSDL, or Web Service Description Language is one commonly
//used format for service discovery, although WCF services can be described in other ways as well.
//It is possible to create REST services (Representative State Transfer) using WCF. These services
//rely on simple HTTP requests to communicate between the client and the server, and because of this
//they can have a smaller footprint than the SOAP messages.  
//When you have identified a service and endpoint that you want to use, and after you know which binding
//you can use and which contracts to adhere to, you can communicate with a WCF service as easily as with an 
//object that you have defined locally. Communications with WCF services can be simple one way transactions,
//request/response messages, or full-duplex communications that can be initiated from either end of the 
//communication channel. 
//You can also use message payload optimization techiques such as Message Transmission Optimization Mechanism,
//(MTOM) to package data if required. 
//The WCF servuce itself might be running in one of a number of different processes on the computer where it
//is hosted. Unlike web services, which always run in IIS(Internet information services, ), you can choose 
//a host process that is approp to your situation. You can use IIS to host WCF services,but you can also use
//WIndows services or executables. If you are using TCP to communicate with a WCF service over a local network, 
//no need to even have IIS installed on the PC that is hosting the service.
//Internet Information Server(IIS) is one of the most popular web servers from Microsoft 
//that is used to host and provide Internet-based services to ASP.NET and ASP Web applications. 
//A web server is responsible for providing a response to requests that come from users.
//When a request comes from client to server IIS takes that 
//request from users and process it and send response back to users.
//You can communicate with WCF services throuugh a variety of transport protocols. 5 are defined in the .NET 4.5
//Framework:
//HTTP => Enables you to communicate with WCF services from anywhere included across the Internet.
//        You can use HTTP communications to create WCF web services
//TCP => Enables you to communicate with WCF services on your local network or across thhe Internet if you
//       configure your firewall approp. TCP is more efficient than HTTP and has more capabilities
//       but can be more complicated to configure
//UDP => User Datagram Protocol is similar to TCP in that it enables communications via the local network or
//       Internet. Service can broadcast messages to multiple clients simultaneously
//Named Pipe => Enables you to communicate with WCF services that are on the same machine as the calling code,
//              but reside on a seperate process.
//MSMQ => Microsoft Message Queueing is a queueing technology that enables messsages sent by an application
//        to be routed through a queue to arrive at a desitination. MSMQ is a reliable messaging technology
//        that ensures that a message sent to a queue will reach that queue. MSMQ is also asynchronous 
//        so a queuence message will be processed only when messages ahead of it in the queue has been
//        processed and a processing service is available.
//Protocols often enabale you to establish  secure connections. HTTPS protocol to establish SSL connections
// across the internet (Secure Sockets Layer). TCP offers extensive possibilites for security
// in a local network by using Windows security framework. UDP doesnt support security.

//In order to connect to a WCF service, you need to know the address of its endpoint. Type of address depends 
//on the protocol you are using. 
//HTTP => Addresses for these protocols are URLs http://<server>:<port>/<service>. For SSL connections you
//can also use https://<server>:<port>/<service>. If you are hosting service in IIS, <service> will be a 
//file with an .svc extension. Subdirectories divide by / character
//TCP => net.tcp://<server>:<port>/<service>
//UDP => soap.udp://<server>:<port>/<service>
//Named Pipe: Dont have port number => net.pipe//<server>/<service>
//The adress for a service is a base address that you can use to create addresses for endpoints 
// representing operations
//net.tcp://<server>/<service>/operation1
//For example, imagine you create a WCF service with a single operation that has bindings for all three 
//protocols listed here. You might use following base addresses:
//http://www.mydomain.com/services/amazingservices/mygreatservice.svc
//net.tcp://myhugeserver:8080/mygreatservice
//net.pipe://localhost/mygreatservice
//You can then use following addresses for operations
//http://www.mydomain.com/services/amazingservices/mygreatservice.svc/greatop   
//net.tcp://myhugeserver:8080/mygreatservice/greatop
//net.pipe://localhost/mygreatservice/greatop
//you can also configure endpoints or use default endpoint
//Bindings specify more than just the transport protocol that will be used by an operation, you can also
//use them to specify the security requirements for communication over the trasport protocol, 
//transactional capabilities of the endpoint, message encoding and much more. 
//.Net provides bindings that you can use and also tweak them to obtain exactly the type of binding that you want
//-up to a point. The predefined bindings have certain capabilties to whcih you must adhere. Each bidning 
//type is represented by a class in the System.ServiceModel namespace. Binding Types:
//BasicHttpBinding => Simplest Http Binding, default binding used by web services, limited security 
                      //and no trasactional support => Default binding for HTTP
//WSHttpBinding => More advanced, allows you to use functionality introduced in WSE
//WSDualHttpBinding =>Extends WSHttpBinding to include duplex communication capabilities, servers 
//                    can initiate communication with the client in addition to ordinary message
//                    exchange. 
//WSFederationHttpBinding => Extends WSHttpBinding capabilities to include federation capabilities,
//                           Fed enables third parties to implement single sign on and other 
                            //security measures
//NetTcpBinding =>  Enables you to configure security, transactions, and so on => Default binding for TCP
//NetNamedPipeBindig => Used for named pipe communications and enables you to configure security,
//                      transactions, and so on. => Default binding for Named pipe
//NetMsmqBinding => default binding for MSMQ
//NetPeerTcpBinding => Used for peer to peer binding
//WebHttpBinding => Used for webservices that use HTTP requests instead of SOAP messages. 
//UdpBinding => Allows binding of UDP protocol => Default binding for UDP

    //Contracts define how WCF services can be used
    //Service Contract => Contains general info about a service  and the operations exposed by a service. For instance 
    //namespace used by service. Services have unique namespaces that are used when defining the schema for SOAP messages
    //in order to avoid conflict with other services. 
    //Operation Contract => Defines how an operation is used. Include the parameter, and return types for method along with
    //additional info such as whether a response message will be returned.
    //Message Contract => Enables you to customize how info is formatted inside SOAP messages - for ex, whether data 
    //should be included in the SOAP header or SOAP message body => useful when creating a WCF service that must integrate
    //with legacy systems. 
    //Fault contract => Defines fault that an operation can return. Whn you use .NET clients, faults result in exceptions
    //that you can catch and deal with in the normal way
    //Data contract => if you use complex types, such as user-defined structs and objects as parameters or return types
    //for operations, then you must define data contrats for these types. Data contracts define the types in terms of 
    // the data that they expose through properties. 
    //Typically add contracts to service classes and methods by using attributes, as you will se later in this chapter

    //Message Patterns =>. There are three types of message patterns.
    //Request/Response messaging -> THe ordinary way of exchaning messages whereby every message sent to a service 
    //results in a response being sent back to the client. Does not mean client waits for response, asynchronous 
    //Simplex messaging => Messages are sent from the client to the WCF operation but no response sent
    //Duplex messaging => Client effectively acts as a server as well as a client, and the server as a client as well
    //as a  server. Enables both the client and server to send messages to each other, which might not have responses.

    //Behaviours. 
    //Addiontional configuration, by adding behaviour to a service, you can control how it is instantiated and used by 
    // its hosting process, how it participates in trasactions, how multithreading issues are dealt with in the service,
    //Operation behaviours can control whther impersonation is used in the operation execution, how the individual 
    //operation affects transactions, and more. 
    //Provide defaults and override settings where necessary, which reduces the amt of configuration required.

    //Hosting
    //WCF services can be hosted in several different processes:
    //Web Server -> IIS hosted WCF services are the closest thing to web services that WCF offers. However, you can use
    //advanced functionality and security features in WCF services that are much more difficult to implement in web
    //services. You can also integrate IIS features such as IIS security. 
    //Executable => you can host a WCF service in any application type that you can create in .NET, such as console 
    //applications,WPF applications
    //Windows Service - YOu can host a WCF service in a Windows service, which means tthat you can use the useful 
    //features that Windows services provide. This includes automatic startup and fault recovery. 
    //Windows Activation Service(WAS) - Designed specifiically to host WCF services, WAS is basically a simple 
    //version of IIS that you can use where IIS is not available. 

    //IIS and WAS provide useful features for WCF services such as activation, process recycling, object pooling. If you use
    // other two hosting options, then WCF is self-hosted. Occasionally self-hosting services for testing, can be other good
    //reasons for creating self-hosted production-grade services. Not allowed to install web server on computer
    //on which your service should run? => might be case if the service runs on a domain controller or if the local 
    //policy of your organization simply prohibits running IIS. => tHEN HOST SERVICE IN WINDOWS SERVICE WORK AS GOOD

    //Message Contracts
    //To use these, define a class that represents the message and apply the MessageContractAttribute attribute to the class.
    //You then apply MessageBodyMemberAtrribute, MesssageHeaderAttribute, or MessageHeaderArrayAttribute attributes to members of 
    //the class. All these attributes in System.ServiceModel namspeace. 

    //Fault Contracts
    //If you have a particular exception type, for ex, a custom exception, that you want to make avaibale to client applications, 
    //then you can apply the System.ServiceModel.FaultContractAttribute attrbute to the operation that migh generate this exception 


namespace WCF_TUT
{
    //Interface defn rie here that defines the service contract and 2 operation contracts 
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and 
    //config file together.
    [ServiceContract]//ServiceContract attribute => interface completely described in metadata for the service, and can be
    //recreated in client applications. 
    //Service contracts are define by applying the System.ServiceModel.ServiceContractAttribute attribute to an interface defn
    //Customize service with these properties:
    //Name => Specifies tha neame of the service contract as defined in the <portType> element in WSDL
    //Namespace => Defines the namespace of the service contract used by the <portType> element in WSDL
    //ConfigurationName => The name of the service contract as used in the configuration file
    //HasProtectionLevel => Determines whether messages used by the service have explicitely define protection levels. Protection levels
    //                      enable you to sign, or sign and encrypt, messsages.
    //ProtectionLevel => The protection level to use for message protection
    //SessionMode => Determines whether sessions are enabled for messages. If you use sessions, then you can ensure that messages sent
    //               to different endpoints of a service are correlated -- that is, they use the same service instance and so can share
    //               state, and so on.
    //CallBackContract => For duplex messaging the client exposes a contract as well as the service. The client in duplex communications
    //                    also acts as a server. This property enables you to specify which contract the client uses.

    public interface IService1
    {
        //You define members as operations by applying the System.ServiceMode.OperationContractAttribute attribute. Properties:
        //Name => Specifies the name of the service operation. The default is the member name. 
        //IsOneWay=>Specifies whether the operation returns a response. If you set this to true, then clients wont wait for the 
        //          operation to complete before continuing
        //AsyncPattern => If set to true, the operation is implemented as two methods that you can use to call the operation 
        //                asynchronously: Begin<methodName>() and End<methodName>()
        //HasProtectionLevel
        //ProtectionLevel
        //IsInitiating => If sessions are used, then this property determines whether calling this operation can start  a new session
        //IsTerminating => If sessions are used, then this property determines whether calling this operation terminates the current session
        //Action => If you are using addressing(an advanced capability of WCF services), then an operation has an associated action name,
        //          which you can specify with this property
        //ReplyAction => As with action but specifies the action name for the operation response
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);//Use compositeType data contract

        // TODO: Add your service operations here
    }

    //A class definition, CompositeType that defines a data contract used by the service
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //You can see that the data contract is simply a class definition that includes the DataContract attribute
    //on the class and DataMember attribute on class members
    //This data contract is exposed to the client application through metadata. This enables client applications 
    //to define a type that can be serialized into a form that can be deserialized by the service into a 
    //CompositeType object
    //Client doesnt need to know actual defn of this type, in fact, the class used by the client 
    //might have a dif implementation 
    //Simple way of defining data structures enables the exchange of complex data structures between
    //WCF service and its clients.
    [DataContract]//data contract CompositeType
    //To define a data contract for a service, you apply the DataContractAttribute attribute to a class definition
    //Attribute found in System.Runtime.Serialization namespace
    //COnfigure attribute with these properties:
    //Name => Names the data contract with a different name than the one you use for the class defn. This name will be used in 
    //        SOAP messages and client side data objects that are defined from the service metadata
    //Namespace =>Defines the namespace that the data contract uses in SOAP messages
    //IsReference => Affects the way that objects are serialized. If set to true, then an object instance is serialized only once
    //               even if it is referenced several times which can be important in some situations, the default is false. 

    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        //Each class member must use a DataMemberAttribute, which is also found in the System.Runtime.Serialization namespace. Properties:
        //Name => Specifies name of the data member when serialized(the default is the member name)
        //IsRequired => Specifies whether the membber must be present in SOAP messages.
        //Order => An int value specifying the order of serializing or deserializing the member which might be required
        //         if one member must be present before another can be understood. Lower Order members are processed first.
        //EmitDefaultValue => Set this to false to prevent memebers from being included in SOAP messages if their value is the default 
        //                    value for the member

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
