// MonitorSample.cs
// This example shows use of the following methods of the C# lock keyword
// and the Monitor class 
// in threads:
//      Monitor.Pulse(Object)
//      Monitor.Wait(Object)
using System;
using System.Threading;
/*
Monitor provides a mechanism that synchronizes access to objects. 
It can be done by acquiring a significant lock so that only one thread can enter
in a given piece of code at one time. Monitor is no different from lock but the monitor
class provides more control over the synchronization of various threads trying to access
the same lock of code.

Using a monitor it can be ensured that no other thread is allowed to access 
a section of application code being executed by the lock owner,
unless the other thread is executing the code using a different locked object.

The Monitor class has the following methods for the synchronize access 
to a region of code by taking and releasing a lock:
 

Monitor.Enter 
Monitor.TryEnter
Monitor.Exit.
Monitor locks objects (that is, reference types), not value types. 
While you can pass a value type to Enter and Exit, it is boxed separately for each call.

Wait releases the lock if it is held and waits to be notified. When Wait is notified,
it returns and obtains the lock again. Both a Pulse and PulseAll signal for the next 
thread in the wait queue to proceed.

The following is the syntax for using a monitor.
*/

/*try  
{  
    int x = 1;

Monitor.Enter(x);  
    try  
    {  
        // Code that needs to be protected by the monitor.  
    }  
    finally  
    {  
          
       Monitor.Exit(x);  
    }  
}  
catch (SynchronizationLockException SyncEx)  
{  
    Console.WriteLine("A SynchronizationLockException occurred. Message:");  
    Console.WriteLine(SyncEx.Message);  
}  


class Program  
{  
     static readonly object _object = new object();  
  
     public static void PrintNumbers()  
     {  
         Boolean _lockTaken = false;  
  
         Monitor.Enter(_object,ref _lockTaken);  
         try  
         {  
            for (int i = 0; i < 5; i++)  
            {  
               Thread.Sleep(100);  
               Console.Write(i + ",");  
            }  
            Console.WriteLine();  
         }  
         finally  
         {  
            if (_lockTaken)  
            {  
               Monitor.Exit(_object);  
            }  
         }  
     }
}  



*/

class Account
{
    
    //In general, avoid locking on a public type, or instances beyond your code's control. 
    //The common constructs lock (this), lock (typeof (MyType)), and lock ("myLock") violate this guideline:
    //lock (this) is a problem if the instance can be accessed publicly.
    //lock (typeof (MyType)) is a problem if MyType is publicly accessible.
    //lock("myLock") is a problem because any other code in the process using the same string, will share the same lock.
    //Best practice is to define a private object to lock on, or a private static object variable to protect data common
    //to all instances. You can't use the await keyword in the body of a lock statement.

//Lock equivalent to:
//   Monitor.Enter(object);
//     try
//   {
// Your code here...
//  }
//    finally
// {
//   Monitor.Exit(object);
// }

private Object thisLock = new Object();
    int balance;

    Random r = new Random();

    public Account(int initial)
    {
        balance = initial;
    }

    int Withdraw(int amount)
    {

        // This condition never is true unless the lock statement
        // is commented out.
        if (balance < 0)
        {
            throw new Exception("Negative Balance");
        }

        // Comment out the next line to see the effect of leaving out 
        // the lock keyword.
        lock (thisLock)
        {
            if (balance >= amount)
            {
                Console.WriteLine("Balance before Withdrawal :  " + balance);
                Console.WriteLine("Amount to Withdraw        : -" + amount);
                balance = balance - amount;
                Console.WriteLine("Balance after Withdrawal  :  " + balance);
                return amount;
            }
            else
            {
                return 0; // transaction rejected
            }
        }
    }

    public void DoTransactions()
    {
        for (int i = 0; i < 100; i++)
        {
            Withdraw(r.Next(1, 100));
        }
    }
}

//Other programs code
public class MonitorSample
{
    public static void Main(String[] args)
    {
        Thread[] threads = new Thread[10];
        Account acc = new Account(1000);
        for (int i = 0; i < 10; i++)
        {
            Thread t = new Thread(new ThreadStart(acc.DoTransactions));
            threads[i] = t;
        }
        for (int i = 0; i < 10; i++)
        {
            threads[i].Start();
        }

//Other Programs Code
/////////////////////////////////////////////////////////////////////////////////////////////////////
        int result = 0;   // Result initialized to say there is no error
        Cell cell = new Cell();

        CellProd prod = new CellProd(cell, 20);  // Use cell for storage, 
                                                 // produce 20 items
        CellCons cons = new CellCons(cell, 20);  // Use cell for storage, 
                                                 // consume 20 items

        Thread producer = new Thread(new ThreadStart(prod.ThreadRun));
        Thread consumer = new Thread(new ThreadStart(cons.ThreadRun));
        // Threads producer and consumer have been created, 
        // but not started at this point.

        try
        {
            producer.Start();
            consumer.Start();
            //Join waits for a thread to finish. On the Thread type, we find the Join instance method. 
            //It enables us to wait until the thread finishes. We use the Join method on an array of threads 
            //to implement useful threading functionality.
            producer.Join();   // Join both threads with no timeout
                               // Run both until done.
            consumer.Join();
            // threads producer and consumer have finished at this point.
        }
        catch (ThreadStateException e)
        {
            Console.WriteLine(e);  // Display text of exception
            result = 1;            // Result says there was an error
        }
        catch (ThreadInterruptedException e)
        {
            Console.WriteLine(e);  // This exception means that the thread
                                   // was interrupted during a Wait
            result = 1;            // Result says there was an error
        }
        // Even though Main returns void, this provides a return code to 
        // the parent process.
        Environment.ExitCode = result;
    }
}

public class CellProd
{
    Cell cell;         // Field to hold cell object to be used
    int quantity = 1;  // Field for how many items to produce in cell

    public CellProd(Cell box, int request)
    {
        cell = box;          // Pass in what cell object to be used(using same cell)
        quantity = request;  // Pass in how many items to produce in cell
    }
    public void ThreadRun()
    {
        for (int looper = 1; looper <= quantity; looper++)
            cell.WriteToCell(looper);  // "producing"
    }
}

public class CellCons
{
    Cell cell;         // Field to hold cell object to be used
    int quantity = 1;  // Field for how many items to consume from cell

    public CellCons(Cell box, int request)
    {
        cell = box;          // Pass in what cell object to be used
        quantity = request;  // Pass in how many items to consume from cell
    }
    public void ThreadRun()
    {
        int valReturned;
        for (int looper = 1; looper <= quantity; looper++)
            // Consume the result by placing it in valReturned.
            valReturned = cell.ReadFromCell();
    }
}

public class Cell
{
    int cellContents;         // Cell contents
    bool readerFlag = false;  // State flag
    public int ReadFromCell()
    {
        lock (this)
        // Enter synchronization block
        //Monitor and lock is the way to provide thread safety in a multithreaded 
        //application in C#. Both provide a mechanism to ensure that only one thread 
        ///is executing code at the same time to avoid any functional breaking of code.
        //The lock keyword ensures that one thread does not enter a critical section of code while another 
        //thread is in the critical section. If another thread tries to enter a locked code, 
        //it will wait, block, until the object is released.

        {
            if (!readerFlag)
            {            // Wait until Cell.WriteToCell is done producing
                try
                {
                    // Waits for the Monitor.Pulse in WriteToCell
                    Monitor.Wait(this);
                }
                catch (SynchronizationLockException e)
                {
                    Console.WriteLine(e);
                }
                catch (ThreadInterruptedException e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("Consume: {0}", cellContents);
            readerFlag = false;    // Reset the state flag to say consuming
                                   // is done.
            Monitor.Pulse(this);   // Pulse tells Cell.WriteToCell that
                                   // Cell.ReadFromCell is done.
        }   // Exit synchronization block
        return cellContents;
    }

    

    public void WriteToCell(int n)
    {
        lock (this)  // Enter synchronization block
        {
            if (readerFlag)
            {      // Wait until Cell.ReadFromCell is done consuming.
                try
                {
                    Monitor.Wait(this);   // Wait for the Monitor.Pulse in
                                          // ReadFromCell
                }
                catch (SynchronizationLockException e)
                {
                    Console.WriteLine(e);
                }
                catch (ThreadInterruptedException e)
                {
                    Console.WriteLine(e);
                }
            }
            cellContents = n;
            Console.WriteLine("Produce: {0}", cellContents);
            readerFlag = true;    // Reset the state flag to say producing
                                  // is done
            Monitor.Pulse(this);  // Pulse tells Cell.ReadFromCell that 
                                  // Cell.WriteToCell is done.
        }   // Exit synchronization block
    }
}