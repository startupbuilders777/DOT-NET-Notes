using System;
using System.Threading.Tasks;
using System.Threading;

class CustomData
{
    public long CreationTime;
    public int Name;
    public int ThreadNum;
}

public class Example
{

    /*public static void Main()
    {
        // Create the task object by using an Action(Of Object) to pass in custom data
        // to the Task constructor. This is useful when you need to capture outer variables
        // from within a loop. 
        Task[] taskArray = new Task[10];
        for (int i = 0; i < taskArray.Length; i++)
        {
            taskArray[i] = Task.Factory.StartNew((Object obj) => {
                CustomData data = obj as CustomData;
                if (data == null)
                    return;

                data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                                 data.Name, data.CreationTime, data.ThreadNum);
            },
                                                  new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
        }
        Task.WaitAll(taskArray);
    }*/

    public static void Main()
    {
        Task<Double>[] taskArray = { Task<Double>.Factory.StartNew(() => DoComputation(1.0)),
                                     Task<Double>.Factory.StartNew(() => DoComputation(100.0)),
                                     Task<Double>.Factory.StartNew(() => DoComputation(1000.0)) };

        var results = new Double[taskArray.Length];
        Double sum = 0;

        for (int i = 0; i < taskArray.Length; i++)
        {
            results[i] = taskArray[i].Result; //result of task computation
                                              // If the Result property is accessed before the computation finishes, 
            //the property blocks the calling thread until the value is available
            Console.Write("{0:N1} {1}", results[i],
                              i == taskArray.Length - 1 ? "= " : "+ ");
            sum += results[i];
        }
        Console.WriteLine("{0:N1}", sum);


        //////////////////////////////////NEW PROGRAM

        //When you use a lambda expression to create a delegate, you have access to all the variables that are visible at 
        //that point in your source code.However, in some cases, most notably within loops, a lambda doesn't capture the 
        //variable as expected. It only captures the final value, not the value as it mutates after each iteration. 
        //The following example illustrates the problem. It passes a loop counter to a lambda expression that instantiates 
        //a CustomData object and uses the loop counter as the object's identifier. As the output from the example shows, 
        //each CustomData object has an identical identifier.
       // Create the task object by using an Action(Of Object) to pass in the loop
       // counter. This produces an unexpected result.
       Task[] taskArrayA = new Task[10];
        for (int i = 0; i < taskArrayA.Length; i++)
        {
            taskArrayA[i] = Task.Factory.StartNew((Object obj) => {
                var data = new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks };
                data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                                  data.Name, data.CreationTime, data.ThreadNum);
            },
                                                 i);
        }
        Task.WaitAll(taskArrayA);

        //You can access the value on each iteration by providing a state object to a task 
        //through its constructor. The following example modifies the previous example by using the loop counter
        //when creating the CustomData object, which, in turn, is passed to the lambda expression. As the output 
        //   from the example shows, each CustomData object now has a unique identifier based on the value of the 
        //  loop counter at the time the object was instantiated.

        // Create the task object by using an Action(Of Object) to pass in custom data
        // to the Task constructor. This is useful when you need to capture outer variables
        // from within a loop. 
        Task[] taskArrayB = new Task[10];
        for (int i = 0; i < taskArrayB.Length; i++)
        {
            taskArrayB[i] = Task.Factory.StartNew((Object obj) => {
                CustomData data = obj as CustomData;
                if (data == null)
                    return;

                data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Task #{0} created at {1} on thread #{2}.",
                                 data.Name, data.CreationTime, data.ThreadNum);
            },
                                                  new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
        }
        Task.WaitAll(taskArrayB);
        //This state is passed as an argument to the task delegate, and it can be accessed from the task object by using the Task.
        //   AsyncState property. The following example is a variation on the previous example.
        //   It uses the AsyncState property to display information about the CustomData objects passed to the lambda expression.

        Task[] taskArrayC = new Task[10];
        for (int i = 0; i < taskArrayC.Length; i++)
        {
            taskArrayC[i] = Task.Factory.StartNew((Object obj) => {
                CustomData data = obj as CustomData;
                if (data == null)
                    return;

                data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
            },
                                                  new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
        }
        Task.WaitAll(taskArrayC);
        foreach (var task in taskArrayC)
        {
            var data = task.AsyncState as CustomData;
            if (data != null)
                Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
                                  data.Name, data.CreationTime, data.ThreadNum);
        }



    }

    private static Double DoComputation(Double start)
    {
        Double sum = 0;
        for (var value = start; value <= start + 10; value += .1)
            sum += value;

        return sum;
    }
}





