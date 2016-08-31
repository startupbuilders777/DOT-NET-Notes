using Ch22Ex03;
using System.ServiceModel;
using static System.Console;
namespace Ch22Ex03Client
{
    /*
    So far, ive seen WCF services hosted in web servers. Enables you to communicate over internet but for the local network communications,
    not efficient way of doing things. For one thing youd need a web server on the computer that hosts the service. Insted do self-hosted.
    A self-hosted WCF service exists in a process that you create, rather than in the process of a specially made hosting application such
    as a web server. This means for example, you can use a console application or Windows application to host your service. 
    
    To self-host a WCF service, you use the System.ServiceModel.ServiceHost Class. You instantiate this class with either the type of the
    service you want to host or an instance of the service class. You can config a service host through properties or methods, or
    (and this is the clever part) through a config file. In fact, host processes, such as web servers, use a ServiceHost instance to do their
    hosting. The difference when self hosting is that you interact with this class directly. However, the config you place in the 
    <system.serviceModel> section of the app.config file for your host application uses exactly the same syntax as the config sections youve
    already seen in this chapter. 

    You can expose a self-hosted service through any protocol that you like, although typically you will use TCP or named pipe binding
    in this type of application. Services accessed through HTTP are more likely to live inside a web server processes, because you get
    the additional functionality that web servers offer. 

    If you want to host a service called MyService, you could use code like this to create an instance of ServiceHost:

    ServiceHost host = new ServiceHost(typeof(MyService));
    
    If you want to host an instance of MyService called myServiceObject, you can do this to create an instance of ServiceHost;
    MyService myServiceObject = new MyService();
    ServiceHost host = new ServiceHost(myServiceObject);
    //WARNING
   Hosting a service instance in a ServiceHost works only if you can config ther service so that calls are always routed to the same object
   instance. To do this, you must apply a ServiceBehvaiourMode property of this attribute to InstanceContextMode.Single
   After creating a ServiceHost instance you can config the service and its endpoints and binding through properties.Alt, if you put config
   in a .config file, the ServiceHost instance will be configed automatically. 
   To start hosting a service once you have configed ServiceHost instance, you use the ServiceHost.Open(), and stop hosting service 
   through ServiceHost.Close
        */

  class Program
  {
    static void Main(string[] args)
    {
      Title = "Ch22Ex03Client";
      WriteLine("Press enter to begin.");
      ReadLine();
      WriteLine("Opening channel.");
      IAppControlService client =
         ChannelFactory<IAppControlService>.CreateChannel(
            new NetTcpBinding(),
            new EndpointAddress(
               "net.tcp://localhost:8081/AppControlService"));
      WriteLine("Creating sun.");
      client.SetRadius(100, "yellow", 3);
      WriteLine("Press enter to continue.");
      ReadLine();
      WriteLine("Growing sun to red giant.");
      client.SetRadius(200, "Red", 5);
      WriteLine("Press enter to continue.");
      ReadLine();
      WriteLine("Collapsing sun to neutron star.");
      client.SetRadius(50, "AliceBlue", 2);
      WriteLine("Finished. Press enter to exit.");
      ReadLine();
    }
  }
}
