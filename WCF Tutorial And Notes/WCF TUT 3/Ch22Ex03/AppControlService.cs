using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Ch22Ex03
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AppControlService" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] //service must use a behaviour attribute reason in client comments
    public class AppControlService : IAppControlService
    {
        //To communicate to Window Class, service needs 
        //reference to application, so you must host an object 
        //instance of the service
        private MainWindow hostApp;
        public AppControlService(MainWindow hostApp)
        {
            this.hostApp = hostApp;
        }
        public void SetRadius(int radius, string foreTo, int seconds)
        {
            hostApp.SetRadius(radius, foreTo, new TimeSpan(0, 0, seconds));
        }
    }
}
