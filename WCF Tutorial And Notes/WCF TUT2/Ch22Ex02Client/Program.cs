using System;
using static System.Console;
using System.ServiceModel;
using Ch22Ex02Contracts;

//Web Services Noteszzz
//Web service is a provision of functionality over the internet, web service provides underlying data. Standarts driven like HTML.
//Solve 2 problems
//Make part of application avaiable past the physical boundary of the application
//Make a dsitributed middle to your application so that you can scale paragraphs if your site suddenly has alot of traffic. 
//3 Principles
//Loosely Coupeled - They dont require a constant connection to the server
//                   No real indication that the messages will ever be delivered, so you should never write software that depnds on the 
//                   delivery of the data from the service. Has to fail gracefully. 
//Contact Driven - They proviide an interface that describes all of their functionality
//                 A web service is required to have an interface that conforms to the Web Services Description Language(WSDL) standard.
//                 This standard states expected inputs and allowed outputs just as an interface would, creating contract between service
//                 provider and client. 
//                 Client machine, at compile time, reads the WSDL and creates a proxy that the client talks to which brokers the 
//                 communication process bet the client and service, provides type safety if supported
//More likely to be chuncky, not chatty - Rather than lots of properties with single values, they provide big methods that return collections
//                 The length of individual calls might be larger, but the number of communicates is fewer, which reduces network overhead
//SOA => Service orientated applications => The user interface calls a service at some point in the process to communicate with the server. 
//                                          Do this for 2 reasons:
//Scability - Main reason to use service inside an application - esp web application. Up to a point most web applicaions have built-in 
//            scaling. If you need more access points, you just add more server and then use a device to sort the trafffic to another 
//            machine. Web applications dif though,because layers have different scalability needs. Sometimes database is loaded, and
//            sometimes web server is. You must be able to seperate the layers of the application, thats where serves start to become
//            useful. Because services use a common format, XML, you can relatively easilty install parts of the application on their own
//            machines to isonalte them physically. Because the functionality of the delted part is called via a service you can scale
//            the application horizontally - from one to multiple servers. 
//Reusability - Every organization has a list of its participants - clients, users, voters, cooks to provide access to.
               //Share data in data silo to multiple people. A database on some server contains all participants demographic information.
               //The database is surrounded by a service layer containing all allowed operations. 
               //The service layer is consumed by all other applications in the system. Services help to provide acces to the data silo. 
               //If you have valuable data, you want to give clients access, you can send data, however then they can use forever.Instead
               //Instead you could track data usage using web service operations
//SOAP => Simple object access protocol is an XML based protocol for sending messages over the internet, usually via HTTP.Think of it as 
// an envelope for remote procedure calls because that is exactly what it is. Global acceptance, longevity, rich experience, lots of dev
//features, trasactions, security. The WS-*(WS star) standards fit here. These Web service standards apply to protocols like SOAP. 
//Additional standards include a lot of new functionality. 
//Web Services Transactions(WS-TX) : Coordinates the outcome of broadly distributed communications
//Web Services Reliabale Exchange(WS-RX) Provides a confirmation of commnication for service calls
//Web Services Federation(WS-FED): Allows for a federation of trush between service providers
//Web Service Remote Portlets(WS-RP): A standard for web parts using services(Like you seen in sharepoint)
//Web Services Security(WS-SX): A supported trusted exchange
//Web Services Discovery(WS-DD) A way to find services in a large enterprise
//Building Information Exchange(oBIX): Allows buildings to talk to each other about thier wiring. 
//OASIS ebXML: A business XML standard that is designed to provide a standardized data model for communications. 
//SOAP has alot of lines because of lots of standards. 200,000 lines in one http call? Soap dramatically increase your overhead in
//communication. For lean code, use REST. 



namespace Ch22E02Client
{
  class Program
  {
        //Difference in this example is that no metadata is required by  the client, as the client has access to the
        //contract assembly.Instead of generating a proxy class from metadata, the client obtains the refence to the
        //service contract interface through an alternative method. Another point to note about this example is the 
        //use of a session to maintain state in the servie, which requires the WSHttpBinding binding istead of the
        //BasicHttpBinding binding


    static void Main(string[] args)
    {
      Person[] people = new Person[]
      {
        new Person { Mark = 46, Name="Jim" },
        new Person { Mark = 73, Name="Mike" },
        new Person { Mark = 92, Name="Stefan" },
        new Person { Mark = 24, Name="Arthur" }
      };
      WriteLine("People:");
      OutputPeople(people);

//The client application has no app.config file to configue the communications with the service and no proxy class defined from metadata
//to communicate with the service. Instead a proxy class is created through the ChannelFactory<T>.CreateChannel() method. This method
//creates a proxy class that implements the IAwardService client, although behind the scenes, the generated class communicates with the
//service just like the metadata-generated proxy shown earlier. Note if you create a porxy class with ChannelFactory<T>.CreateChannel(),
//the communication channel will, by default, time out after a minute, which can lead to communication errors. There are ways to keep
//connections alive, but they are beyond the scope of this chapter. Creating proxy classes this way is extremely useful technique that you
//can use to quickly generate a client application on the fly. 
 
      IAwardService client = ChannelFactory<IAwardService>.CreateChannel(
         new WSHttpBinding(),
         new EndpointAddress("http://localhost:51735/AwardService.svc"));
      client.SetPassMark(70);
      Person[] awardedPeople = client.GetAwardedPeople(people);
      WriteLine();
      WriteLine("Awarded people:");
      OutputPeople(awardedPeople);
      ReadKey();
    }
        

    static void OutputPeople(Person[] people)
    {
      foreach (Person person in people)
        WriteLine("{0}, mark: {1}", person.Name, person.Mark);
    }
  }
}
