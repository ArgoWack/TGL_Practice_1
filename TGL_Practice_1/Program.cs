using System;
using static System.Console;
using System.Threading;
namespace TGL_Practice_1
{
    /*
            Short Summary:

    Program symulates work of Timber company which uses 2 kinds of workers "TimberMan"'s and "Experienced TimberMan"'s and hires 5 workers of each kind.
    Their job is to gather logs. Each workers is separate thread. To avoid sync problems I use lock when storing logs. 
    The work is split into 4 shifts each separated with 5000ms break. The amount of logs gatherd by each worker is based on generated seed and worker grade.
    I also added numbers of Threads info. They aren't essential for program but I thought that it would show better how is it working. 

    */
    class Program
    {
        class Timber_company
        {
            //here logs of all workers are stored 
            public static int log;

            //lock for returning wood
            static Object log_deposition_lock = new Object();
            protected internal abstract class Worker
            {
                public abstract void cut_timber();
                public void rest()
                {
                    WriteLine("Worker is resting");
                }
            }
            protected internal class TimberMan : Worker
            {
                [ThreadStatic]
                static int threadSpecificData;
                public override void cut_timber()
                {
                    //returns number of thread
                    threadSpecificData = Thread.CurrentThread.ManagedThreadId - 4;
                    lock (log_deposition_lock)
                    {
                        Random seed = new Random();
                        //generates random number with [0,9]
                        int chances = seed.Next(0, 10);
                        int gatherd_logs = 0;

                        switch (chances)
                        {
                            case 0:
                            {
                                    // no logs - for example due to some accident
                                    break;
                            }
                            case 1:
                            {
                                    //low logs
                                    gatherd_logs += 1;
                                 break;
                            }
                            case 9:
                            {
                                    //big portion of logs
                                    gatherd_logs += 30;
                                    break;
                            }
                            default:
                            {
                                    //typical logs
                                    gatherd_logs += chances*2;
                                    break;
                            }
                        }
                        log += gatherd_logs;
                        WriteLine("On Thread: " + threadSpecificData + " was gatherd: " + gatherd_logs + " of wood by TimberMan which brings it to total of: " + log + " logs");
                    }
                }
            }
            protected internal sealed class Experienced_TimberMan : TimberMan
            {
                [ThreadStatic]
                static int threadSpecificData;
                public override void cut_timber()
                {
                    //returns number of thread
                    threadSpecificData = Thread.CurrentThread.ManagedThreadId - 4;
                    lock (log_deposition_lock)
                    {
                        Random seed = new Random();
                        //generates random number with [0,9]
                        int chances = seed.Next(0, 10);
                        int gatherd_logs=0;

                        switch (chances)
                        {
                            case 0:
                                {
                                    // no logs - for example due to some accident
                                    break;
                                }
                            case 1:
                                {
                                    //low logs
                                    gatherd_logs += 2;
                                    break;
                                }
                            case 9:
                                {
                                    //big portion of logs
                                    gatherd_logs += 50;
                                    break;
                                }
                            default:
                                {
                                    //typical log
                                    gatherd_logs += chances * 3;
                                    break;
                                }
                        }
                        log += gatherd_logs;
                        WriteLine("On Thread: "+threadSpecificData +" was gatherd: " + gatherd_logs + " of wood by Experienced TimberMan which brings it to total of: "+ log+" logs");
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            try
            {
                Timber_company.TimberMan start_timberMans = new Timber_company.TimberMan();
                Timber_company.Experienced_TimberMan start_Experienced_TimberMans = new Timber_company.Experienced_TimberMan();

                int work_shifts = 1;
                // iteration thro work_shifts. Set on 4 but could be any number. 
                while (work_shifts < 5)
                {
                    WriteLine("Start of " + work_shifts + " work shift");

                    //5 timberMan's threads
                    Thread TimberMan_1 = new Thread(start_timberMans.cut_timber);
                    Thread TimberMan_2 = new Thread(start_timberMans.cut_timber);
                    Thread TimberMan_3 = new Thread(start_timberMans.cut_timber);
                    Thread TimberMan_4 = new Thread(start_timberMans.cut_timber);
                    Thread TimberMan_5 = new Thread(start_timberMans.cut_timber);

                    //5 Experienced timberMan's threads
                    Thread Experienced_TimberMan_1 = new Thread(start_Experienced_TimberMans.cut_timber);
                    Thread Experienced_TimberMan_2 = new Thread(start_Experienced_TimberMans.cut_timber);
                    Thread Experienced_TimberMan_3 = new Thread(start_Experienced_TimberMans.cut_timber);
                    Thread Experienced_TimberMan_4 = new Thread(start_Experienced_TimberMans.cut_timber);
                    Thread Experienced_TimberMan_5 = new Thread(start_Experienced_TimberMans.cut_timber);

                    TimberMan_1.Start(); TimberMan_2.Start(); TimberMan_3.Start(); TimberMan_4.Start(); TimberMan_5.Start();
                    Experienced_TimberMan_1.Start(); Experienced_TimberMan_2.Start(); Experienced_TimberMan_3.Start(); Experienced_TimberMan_4.Start(); Experienced_TimberMan_5.Start();

                    TimberMan_1.Join(); TimberMan_2.Join(); TimberMan_3.Join(); TimberMan_4.Join(); TimberMan_5.Join();
                    Experienced_TimberMan_1.Join(); Experienced_TimberMan_2.Join(); Experienced_TimberMan_3.Join(); Experienced_TimberMan_4.Join(); Experienced_TimberMan_5.Join();

                    if (work_shifts < 4)
                    {
                        WriteLine("Break Time");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        WriteLine("Work is over");
                    }
                    work_shifts++;
                }
                WriteLine("During the day " + Timber_company.log + " logs were gatherd.");
                ReadKey();
            }
            catch(Exception ex)
            {
                WriteLine("Somethign went wrong. Details: "+ex);
                ReadKey();
            }
        }
    }
}
