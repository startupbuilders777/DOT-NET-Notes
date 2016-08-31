/*A task that does not return a value is represented by the System.Threading.Tasks.Task 
    class. A task that returns a value is represented by the System.Threading.Tasks.Task<TResult> class, 
which inherits from Task.The task object handles the infrastructure details and provides methods and properties 
    that are accessible from the calling thread throughout the lifetime of the task.For example, you can access 
    the Status property of a task at any time to determine whether it has started running, ran to completion, 
    was canceled, or has thrown an exception.The status is represented by a TaskStatus enumeration.
When you create a task, you give it a user delegate that encapsulates the code that the task will 
    execute.The delegate can be expressed as a named delegate, an anonymous method, or a lambda expression. 
        Lambda expressions can contain a call to a named method, as shown in the following example.Note that
        the example includes a call to the Task.Wait method to ensure that the task completes execution before 
        the console mode application ends.*/

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class Example
{


    static void sayThis() => Console.WriteLine("i LIKE FOODSZ");


    /* static void Main()
     {
         // Retrieve Darwin's "Origin of the Species" from Gutenberg.org.
         string[] words = CreateWordArray(@"http://www.gutenberg.org/files/2009/2009.txt");
         #region ParallelTasks
         // Perform three tasks in parallel on the source array
         Parallel.Invoke(() =>
         {
             Console.WriteLine("Begin first task...");
             GetLongestWord(words);
         }, // close first Action
         () =>
         {
             Console.WriteLine("Begin second task...");
             GetMostCommonWords(words);
         }, //close second Action
         () =>
         {
             Console.WriteLine("Begin third task...");
             GetCountForWord(words, "species");
         } //close third Action
         ); //close parallel.invoke
         Console.WriteLine("Returned from Parallel.Invoke");
         #endregion
         Console.WriteLine("Press any key to exit");
         Console.ReadKey();

        // Parallel.Invoke(ParallelOptions, Action[]) – Executes each of the provided tasks/actions, 
         //possibly in parallel, unless the operation is cancelled by the user.

//            This method enables you to configure the behavior of the operation. 
  //          Using ParallelOptions, you can configure CancellationToken, MaxDegreeOfParallelism 
   //         – maximum number of concurrent tasks enabled, and TaskScheduler. Here one thing is to 
    //        note that if you configure the value of MaxDegreeOfParallelism as say n then framework 
     //       will require maximum n or less threads to process the task.
     static void Main(string[] args)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken ct = cts.Token;
                ParallelOptions po = new ParallelOptions { CancellationToken = ct, MaxDegreeOfParallelism = System.Environment.ProcessorCount };

                Parallel.Invoke(po,
                        new Action(() => DoWork(1, ct)),
                        new Action(() => DoWork(2, ct)), 
                        new Action(() => DoWork(3, ct)),
                        new Action(() => DoWork(4, ct)),
                        new Action(() => DoWork(5, ct)),
                        new Action(() => DoWork(6, ct)),
                        new Action(() => { cts.Cancel(); }), 
                        new Action(() => DoWork(7, ct)),
                        new Action(() => DoWork(8, ct))
                    );                
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey()
            }

        Task Parallelism Framework provides you different flavours of Task class object creation. 
        In this way, you can create different types of Tasks for specific scenarios.

Task constructors which can be used to create task with no return value. However you have control over task behaviour.

Task(Action) – Initializes a new Task with the specified action.
Task(Action, CancellationToken) – Initializes a new Task with the specified action and CancellationToken.
Task(Action, TaskCreationOptions) – Initializes a new Task with the specified action and creation options.
Task(Action<Object>, Object) – Initializes a new Task with the specified action and state.
Task(Action, CancellationToken, TaskCreationOptions) – Initializes a new Task with the specified action and creation options.
Task(Action<Object>, Object, CancellationToken) – Initializes a new Task with the specified action, state, and options.
Task(Action<Object>, Object, TaskCreationOptions) – Initializes a new Task with the specified action, state, and options.
Task(Action<Object>, Object, CancellationToken, TaskCreationOptions) – Initializes a new Task with the specified action, state, and options.
 
        
        
        
        
        
        }*/

    static void printMessage() => Console.WriteLine("wAZZ GOOD IN DA HOODSSZZ");

    public static void Main()
    {
        // The Parallel.Invoke method provides a convenient way to run any number of arbitrary
        ///  statements concurrently. Just pass in an Action delegate for each item of work.The easiest
        //    way to create these delegates is to use lambda expressions.The lambda expression can either 
        //    call a named method or provide the code inline.The following example shows a basic Invoke 
        //   call that creates and starts two tasks that run concurrently.The first task is represented 
        //  by a lambda expression that calls a method named DoSomeWork, and the second task is represented
        //  by a lambda expression that calls a method named DoSomeOtherWork.


        //Parallel.Invoke(() => DoSomeWork(), () => DoSomeOtherWork());
        //Action Delegate: Encapsulates a method that has no parameters and does not return a value.
        // public delegate void Action()

        /*
        Task<TResult> constructors which can be used to create task with return value.
        You have control over task behavior as well. 
        TResult is the type of value returned by the delegate or task.

Task<TResult>(Func<TResult>) – Initializes a new Task<TResult> with the specified function.
Task<TResult>(Func<TResult>, CancellationToken) – Initializes a new Task<TResult> with the specified function.
Task<TResult>(Func<TResult>, TaskCreationOptions) – Initializes a new Task<TResult> with the specified function and creation options.
Task<TResult>(Func<Object, TResult>, Object) – Initializes a new Task<TResult> with the specified function and state.
Task<TResult>(Func<TResult>, CancellationToken, TaskCreationOptions) – Initializes a new Task<TResult> with the specified function and creation options.
Task<TResult>(Func<Object, TResult>, Object, CancellationToken) – Initializes a new Task<TResult> with the specified action, state, and options.
Task<TResult>(Func<Object, TResult>, Object, TaskCreationOptions) – Initializes a new Task<TResult> with the specified action, state, and options.
Task<TResult>(Func<Object, TResult>, Object, CancellationToken, TaskCreationOptions) – Initializes a new Task<TResult> with the specified action, state, and options
        
        
        */



        int[] nums = Enumerable.Range(0, 1000).ToArray();

        var myTasks = nums.Select(num => Task.Factory.StartNew<Tuple<int, long>>(() => new Tuple<int, long>(num, num * num))).ToList();
        // Instead of simple calculation like num*num, you can write code to say download content from some site url

        DateTime threadStartTime = DateTime.Now;
        while (myTasks.Count > 0)
        {
            try
            {
                var waitTime = 2000;
                var index = Task.WaitAny(myTasks.Cast<Task>().ToArray(), (int)waitTime); // Wait for any task to complete and get the index of completed task
                if (index < 0) break;
                var currentTask = myTasks[index];
                myTasks.RemoveAt(index); // Remove the task from list since it is processed
                if (currentTask.IsCanceled || currentTask.IsFaulted) continue;
                if (currentTask.Result.Item1 == 0) continue;

                var input = currentTask.Result.Item1;
                var output = currentTask.Result.Item2;

                // You can process the downloaded content here one by one, which ever gets downloaded first.

                Console.WriteLine("Square root of {0} is {1}", input, output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        Console.WriteLine("Main method complete. Press <enter> to finish.  And start other tasks");
        Console.ReadLine();

        // use an Action delegate and named method
        Task task1 = new Task(new Action(printMessage));
        // use an anonymous delegate
        Task task2 = new Task(delegate { printMessage(); });
        // use a lambda expression and a named method
        Task task3 = new Task(() => printMessage());
        // use a lambda expression and an anonymous method
        Task task4 = new Task(() => { printMessage(); });
        task1.Start();
        task2.Start();
        task3.Start();
        task4.Start();

        Thread.CurrentThread.Name = "Main";

        // Create a task and supply a user delegate by using a lambda expression. 
        Task taskA = new Task(() => Console.WriteLine("Hello from taskA."));
        // Start the task.
        taskA.Start();



        Action taskMethod = new Action(sayThis);
        Task taskB = new Task(taskMethod);
        taskB.Start();


        // Define and run the task.
        //  Task taskA = Task.Run(() => Console.WriteLine("Hello from taskA."));

        // Better: Create and start the task in one operation. 
       // Task taskA = Task.Factory.StartNew(() => Console.WriteLine("Hello from taskA."));

        // Output a message from the calling thread.
        Console.WriteLine("Hello from thread '{0}'.",
                          Thread.CurrentThread.Name);
        taskA.Wait();
        taskB.Wait();
        
    }
}
// The example displays output like the following:
//       Hello from thread 'Main'.
//       Hello from taskA.





