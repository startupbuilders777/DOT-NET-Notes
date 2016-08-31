using System;
using System.Threading;

class MyThread
{
    public int count;
    public Thread thrd;

    public MyThread(string name)
    {
        count = 0;
        thrd = new Thread(this.run);
        thrd.Name = name;
        thrd.Start();
    }

    void run()
    {
        Console.WriteLine(thrd.Name + " starting.");

        do
        {
            Thread.Sleep(500);
            Console.WriteLine("In " + thrd.Name +
                              ", count is " + count);
            count++;
        } while (count < 10);

        Console.WriteLine(thrd.Name + " terminating.");
    }
}

//The thread.join() method is used to call a thread and Blocks the calling thread until a 
//thread terminates i.e Join method waits for finishing other threads by calling its join method.



class MainClass
{

    static void Run()
    {
        for (int i = 0; i < 50; i++) Console.Write("C#corner");
    }

    public static void Main()
    {

        
            Thread th = new Thread(Run);
            th.Start();
            th.Join();
            Console.WriteLine("Thread t has terminated !");
        
        Thread.Sleep(1000);
        ///////
        Console.WriteLine("Main thread starting.");

        MyThread mt1 = new MyThread("Child #1");
        MyThread mt2 = new MyThread("Child #2");
        MyThread mt3 = new MyThread("Child #3");

        mt1.thrd.Join();
        Console.WriteLine("Child #1 joined.");

        mt2.thrd.Join();
        Console.WriteLine("Child #2 joined.");

        mt3.thrd.Join();
        Console.WriteLine("Child #3 joined.");

        Console.WriteLine("Main thread ending.");
    }
}