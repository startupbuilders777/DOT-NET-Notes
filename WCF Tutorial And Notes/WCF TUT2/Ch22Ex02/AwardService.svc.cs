using System.Collections.Generic;
using Ch22Ex02Contracts;
namespace Ch22Ex02
{
    //The only difference between this service class and the one in the first WCF program is that it is stateful. This is permissable 
    //as a session is defined to correlate messages from a client. 
    /*

        <protocolMapping>
          <add scheme="http" binding="wsHttpBinding" />
        </protocolMapping>

        This overrides the default mapping for HTTP Binding. However, be aware that this type of override is applied to all services
        in a project. If you have more than one service in a project, then you would have to ensure that this binding is acceptable
        is acceptable to each of them. 
    */
    public class AwardService : IAwardService
  {
    private int passMark;
    public void SetPassMark(int passMark)
    {
      this.passMark = passMark;
    }
    public Person[] GetAwardedPeople(Person[] peopleToTest)
    {
      List<Person> result = new List<Person>();
      foreach (Person person in peopleToTest)
      {
        if (person.Mark > passMark)
        {
          result.Add(person);
        }
      }
      return result.ToArray();
    }
  }
}
