using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks_1._5
{
    class Program
    {


        static int fibNum(int n) {
            if (n == 2)
                return 1;
            else if (n == 1)
                return 1;
            return fibNum(n - 1) + fibNum(n - 2);
        }

        

        static void Main(string[] args)
        {
            /*
            Some important parameters which can be used while creating task.

CancellationToken – This token is used to cancel the task while task is already running.
TaskCreationOptions – This is used to customize the task’s behavior. Important possible values for TaskCreationOptions are AttachedToParent, DenyChildAttach, LongRunning,PreferFairness.
State object – It represents data to be used by the Action/Func/Task.
Some important properties which can be used to monitor current task.

AsyncState – Gets the state object supplied when the Task was created, or null if none was supplied.
Exception – Gets the AggregateException that caused the Task to end prematurely. If the Task completed successfully or has not yet thrown any exceptions, this will return null.
IsCanceled – Gets whether this Task instance has completed execution due to being cancelled.
IsFaulted – Gets whether the Task completed due to an un-handled exception.
IsCompleted – Gets whether this Task has completed.
Result – Gets the result value of this Task<TResult>. The data-type of Result would be TResult.
Status – Gets the TaskStatus of this task. Possible values are Created, WaitingForActivation, WaitingToRun, Running, Cancelled, Faulted, WaitingForChildrenToComplete, RanToCompletion.
Some important methods which can be used to control current task.

Start() – starts the Task, scheduling it for execution to the current TaskScheduler.
Wait() - waits for the Task to complete execution. This methods has many overloads which accepts CancellationToken, time in milliseconds, or TimeSpan.
    ContinueWith() – start new task after the current task has completed. It provides many constructors suitable for specific requirement.
            RunSynchronously() – runs the Task synchronously on the current TaskScheduler. By default, tasks are run asynchronously.
            */
            // Wait on a single task with no timeout specified.
            Task taskA = Task.Factory.StartNew(() => fibNum(25));
            taskA.Wait();
            Console.WriteLine("taskA has completed.");
            // Wait on a single task with a timeout specified.
            Task taskB = Task.Factory.StartNew(() => fibNum(25));
            taskB.Wait(1000); //Wait for 1000 ms.

            if (taskB.IsCompleted)
                Console.WriteLine("taskB has completed.");
            else
                Console.WriteLine("Timed out before taskB completed.");

            // Wait for all tasks to complete.
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => fibNum(25));
            }
            Task.WaitAll(tasks);

            // Wait for first task to complete.
            Task<double>[] tasks2 = new Task<double>[3];

           // Try three different approaches to the problem. Take the first one.
          tasks2[0] = Task<double>.Factory.StartNew(() => fibNum(22));
          tasks2[1] = Task<double>.Factory.StartNew(() => fibNum(21));
          tasks2[2] = Task<double>.Factory.StartNew(() => fibNum(20));
          int index = Task.WaitAny(tasks2);
           double d = tasks2[index].Result;
            Console.WriteLine("task[{0}] completed first with result of {1}.", index, d);

            // The antecedent task. Can also be created with Task.Factory.StartNew.
            Task<DayOfWeek> taskkA = new Task<DayOfWeek>(() => DateTime.Today.DayOfWeek);

            // The continuation. Its delegate takes the antecedent task as an argument and can return a different type.
            Task<string> continuation = taskkA.ContinueWith((antecedent) =>
            {
                return String.Format("Today is {0}.", antecedent.Result);
                // antecedent.Result gives you the result of previous task in chain
            });

            // Start the antecedent.
            taskkA.Start();

            // Use the contuation's result.
            Console.WriteLine(continuation.Result);

        }
    }
}
