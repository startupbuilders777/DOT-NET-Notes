using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using WCF_TUT_CONSOLE.ServiceReference1; 

namespace WCF_TUT_CONSOLE
{
    //Created a simple web server hosted WCF service and console client
    //The Add Server reference tool creates all the classes you require to access the service. This includes
    //a proxy class for the service that includes methods for all the operations exposed by the service(Service1Client) 
    //and a client side class generated from the data contract(CompositeType)
    //Also adds a config file, app.config. which defines 2 things, Binding information for ther service endpoint 
    //and the Address and contract for the endpoint
    /*
    <? xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version = "v4.0" sku=".NETFramework,Version=v4.5.2" />
    </startup>
    <system.serviceModel>
    <!-- The <binding> element which has the name BasicHttpBinding_IService1 is included so that you can use it to customize the
    configuration of the binding, number of settings ranging from timeout settings to message size limits and security.
    If these had been specified in the service project to be nondefault values, then you would have seen them in this file
    since they would have copied across. In order for client to communicate with service, the binding configurations must match
    --> 
        <bindings> //Binding info taken from the service description
            <basicHttpBinding>
                <binding name = "BasicHttpBinding_IService1" />
            </ basicHttpBinding >
        </ bindings >
        <!-- Binding is used in the endpoint config along with the base address of ther serrvice(which is the address of the .svc -->
        <!-- file forr web server-hosted services) and the client-side version of the contract IService1: -->
        <!-- If you remove the <bindings> section, as well as the bindingConfiguration attribute from the <endpoints> element -->
        <!-- then the client will use the default binding configuration -->

        < client >
            < endpoint address="http://localhost:60598/Service1.svc" 
              binding="basicHttpBinding"
              bindingConfiguration="BasicHttpBinding_IService1" 
              contract="ServiceReference1.IService1"
              name="BasicHttpBinding_IService1" />
        </client>
    </system.serviceModel>
</configuration>
*/
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Ch22EEx01Client";
            string numericInput = null;
            int intParam;
            do {

                WriteLine("Enter an integer and press enter to call the WCF service");
                numericInput = ReadLine();
            } while (!int.TryParse(numericInput, out intParam));
            Service1Client client = new Service1Client();
            WriteLine(client.GetData(intParam));//used one of the operations.
            WriteLine("Press Any Key To Exit");
            ReadKey();
        }
    }
}
