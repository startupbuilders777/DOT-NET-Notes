using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

//It can be tricky to test services propertly since client application may be complex
//You can test the service without using client, by setting the service as the startup project rather than the client, and
//right clicking Service1.svc service and clicking set As Startup page and running the program
//Visual studio provides test tool to ensure WCF operations work correctly, and tool automatically configured to work with your
//WCF service projects
//All you need to do is ensure the service you want to test(that is the svc file) is set to be the statup page for the WCF service
//project. You can run test client as standalone application  by going to C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\WCFTestClient.exe
//In Web.config, ensure that metadata is enabled: 
//<serviceMetadata httpGetEnabled = "true" httpsGetEnabled = "true"/>
//Use WCF test client to inspect and invoke an operation on the service you created
//Slight delay is b/c tool has to inspect the service to determine its capabilities, uses same metadata as Add Service Reference tool,
//Next looked at configuration used to access the service, generated automatically from the service metadata. You can edit this. 
//Finally invoked operation, allows you to enter paramters then displays result all without you writing any client code
//You can also see the actual XML that is sent and recieved to obtain the result
namespace WCF_TUT
{
    //Service1.svc file that defines the hosting for the service => to see this code right click and view markup. This is what it shows:
    //<%@ ServiceHost Language="C#" Debug="true" Service="WCF_TUT.Service1" CodeBehind="Service1.svc.cs" %>
    //This is a ServiceHost instruction that is used to tell the web server (the developement web server in this case but also applied to IIS)
    //what service is hosted at this address => necessary to obtain the hosting features of the web server 
    //The class that defines the service is declared in the Service attribute and the code file that defines the class is  
    //declared in the CodeBehind attribute
    //Obv, this file is not req for WCF services that aren't hosted in a web server.

    //There is also a <system.serviceModel> config section (in Web.config) that configures the service
    //Configure all types of WCF services hosted or self hosted as well as clients of WCF services using Web.config
    //The vocabulary of this configuration is such that you can apply pretty mucch any config that you can think of
    //to a service, and you can even extend this syntax
    //WCF config code is contained in the <system.serviceModel> section of a Web.config or app.config files. 
    //Not alot of config done in ours, as default values used.
    //The configuration section consists of a single subsection that supplies overrides to default values for
    //the service behaviour <behaviours>
    //If non default configuraitons were being used, you would expect to see a <services> section inside <system.ServiceModel>
    //containing one or more <services> child sections, in turn, the <service> sections can contain child <endpoint> sections,
    //each of which, defines an endpoint for the service. In fact, the endpoints defined are base endpoints for the service.
    //Endpoints for operations are inferred from these
    /*
      <system.serviceModel>
    <behaviors> //This section can define one ore more behaviours in <behaviour> child sections, which can be reused on multiple other
                //elements. A <behaviour> section can be given a name to facilitate this reuse(so that it can be referenced from
                //elsewhere), or can be used without a name(as in this example) to specify overrides to default behaviour settings
      <serviceBehaviors>
        <behavior> 
          <!-- To avoid disclosing metadata information, set the values below to false before deployment -->
          <!-- This default behaviour override relates to meta-data. Metadata is ussed to enable clients to obtain -->
          <!-- descriptions of WCF services. The default config defines two default endpoints for services -->
          <!-- One is the endpoint that clients use to access the service, the other is an endpoint  -->
          <!-- used to obtain metadata from the service. Default value is false for each. If you remove this line of code-->
          <!-- automatically sets to false-->
          <!-- If you disable, wont stop cleint from being able to access the service b/c it already obtained the metadata-->
          <!-- it needed when you added the service reference. Disabling will stop other clients from using the Add Service Reference-->
          <!-- tool for this service.-->
          <!-- Without metadata, another common way to access a WCF service is to define its contracts in a seperate assembly-->
          <!-- which is referenced by both the hosting project and the client project. The client can thne generate a proxy-->
          <!-- by using these contracts directly rather than through exposed metadata-->
          <serviceMetadata httpGetEnabled= "true" httpsGetEnabled="true"/>
        <!-- To receive exception details in faults for debugging purposes, set the value below to true.  -->
        <!-- Set to false before deployment to avoid disclosing exception information -->
        <!-- Setting can be set to true to expose exception details in any faults that are transmitted to the client -->
          <serviceDebug includeExceptionDetailInFaults="false"/>
        </behavior>
      </serviceBehaviors>
    </behaviors>
    <protocolMapping>
        <add binding="basicHttpsBinding" scheme="https" />
    </protocolMapping>    
    <serviceHostingEnvironment aspNetCompatibilityEnabled="true" multipleSiteBindingsEnabled="true" />
  </system.serviceModel>
        */



    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    //Class defn rie here that defines the functionality of ther service
    //Note that this class deoesnt need to require any particular attributes, all it needs to do is implement the interface that
    //defines the service contract
    //In fact you can add attributes to this class and its members to specify behaviours but these arent mandatory
    //The seperation of the service contract(the interface) from the service implementation (the class) works 
    //well.The client doesnt need to know anything about the class, which could include much more functionality
    //than just the service implementation. A single class could implement more than one service contract. 
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
