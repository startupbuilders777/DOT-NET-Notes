using System.ServiceModel;
namespace Ch22Ex02Contracts
{
    //The SessionMode property of this attribute is set to SessionMode.Required as this service requires state.
    [ServiceContract(SessionMode = SessionMode.Required)]
  public interface IAwardService
  {
 //The first operation is the one that sets state, and thereore has the initiating property set to true.
 //This operation doesnt return anything so it is defined as a one way operation .
    [OperationContract(IsOneWay = true, IsInitiating = true)]
    void SetPassMark(int passMark);

    //This operation uses the data contract defined
    [OperationContract]
    Person[] GetAwardedPeople(Person[] peopleToTest);
  }
}
