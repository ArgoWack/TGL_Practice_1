using System;
using static System.Console;
using System.Threading;
namespace TGL_Practice_1
{
    class Program
    {
        class Timber_company
        {
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
                    threadSpecificData = Thread.CurrentThread.ManagedThreadId - 4;
                    lock (log_deposition_lock)
                    {
                        Random seed = new Random();

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
                    threadSpecificData = Thread.CurrentThread.ManagedThreadId - 4;
                    lock (log_deposition_lock)
                    {
                        Random seed = new Random();

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
                while (work_shifts < 5)
                {
                    WriteLine("Start of " + work_shifts + " work shift");
                    Thread TimberMan_1 = new Thread(start_timberMans.cut_timber);
                    Thread TimberMan_2 = new Thread(start_timberMans.cut_timber);
                    Thread TimberMan_3 = new Thread(start_timberMans.cut_timber);
                    Thread TimberMan_4 = new Thread(start_timberMans.cut_timber);
                    Thread TimberMan_5 = new Thread(start_timberMans.cut_timber);

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
