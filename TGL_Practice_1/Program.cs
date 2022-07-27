using System;
using static System.Console;
using System.Threading;
using System.Collections.Generic;
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
        class TimberCompany
        {
            //here logs of all workers are stored 
            protected internal static int log;

            protected internal static List<int> timberMansLogsPerShift = new List<int>();
            protected internal static List<int> experiencedTimberMansLogsPerShift = new List<int>();

            protected internal static int logsGatherdByTimberMansWithinShift;
            protected internal static int logsGatherdByExperiencedTimberMansWithinShift;

            //lock for returning wood
            static Object logDepositionLock = new Object();
            protected internal abstract class Worker
            {
                public abstract void cutTimber();
                public void rest()
                {
                    WriteLine("Worker is resting");
                }
            }
            protected internal class TimberMan : Worker
            {
                [ThreadStatic]
                static int threadSpecificData;
                public override void cutTimber()
                {
                    //returns number of thread
                    threadSpecificData = Thread.CurrentThread.ManagedThreadId - 4;
                    lock (logDepositionLock)
                    {
                        Random seed = new Random();
                        //generates random number with [0,9]
                        int chances = seed.Next(0, 10);
                        int gatherdLogs = 0;

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
                                    gatherdLogs += 1;
                                 break;
                            }
                            case 9:
                            {
                                    //big portion of logs
                                    gatherdLogs += 30;
                                    break;
                            }
                            default:
                            {
                                    //typical logs
                                    gatherdLogs += chances*2;
                                    break;
                            }
                        }
                        logsGatherdByTimberMansWithinShift += gatherdLogs;
                        log += gatherdLogs;
                        WriteLine("On Thread: " + threadSpecificData + " was gatherd: " + gatherdLogs + " of wood by TimberMan which brings it to total of: " + log + " logs");
                    }
                }
            }
            protected internal sealed class Experienced_TimberMan : TimberMan
            {
                [ThreadStatic]
                static int threadSpecificData;
                public override void cutTimber()
                {
                    //returns number of thread
                    threadSpecificData = Thread.CurrentThread.ManagedThreadId - 4;
                    lock (logDepositionLock)
                    {
                        Random seed = new Random();
                        //generates random number with [0,9]
                        int chances = seed.Next(0, 10);
                        int gatherdLogs = 0;

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
                                    gatherdLogs += 2;
                                    break;
                                }
                            case 9:
                                {
                                    //big portion of logs
                                    gatherdLogs += 50;
                                    break;
                                }
                            default:
                                {
                                    //typical log
                                    gatherdLogs += chances * 3;
                                    break;
                                }
                        }
                        logsGatherdByExperiencedTimberMansWithinShift += gatherdLogs;
                        log += gatherdLogs;
                        WriteLine("On Thread: "+threadSpecificData +" was gatherd: " + gatherdLogs + " of wood by Experienced TimberMan which brings it to total of: "+ log+" logs");
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            try
            {
                TimberCompany.TimberMan startTimberMans = new TimberCompany.TimberMan();
                TimberCompany.Experienced_TimberMan startExperiencedTimberMans = new TimberCompany.Experienced_TimberMan();

                int workShifts = 1;

                const int amountOfShifts = 5;
                // iteration thro work_shifts. Set on 5 but could be any number. 
                while (workShifts <= amountOfShifts)
                {
                    WriteLine("Start of " + workShifts + " work shift");

                    //5 timberMan's threads
                    Thread TimberMan1 = new Thread(startTimberMans.cutTimber);
                    Thread TimberMan2 = new Thread(startTimberMans.cutTimber);
                    Thread TimberMan3 = new Thread(startTimberMans.cutTimber);
                    Thread TimberMan4 = new Thread(startTimberMans.cutTimber);
                    Thread TimberMan5 = new Thread(startTimberMans.cutTimber);

                    //5 Experienced timberMan's threads
                    Thread ExperiencedTimberMan1 = new Thread(startExperiencedTimberMans.cutTimber);
                    Thread ExperiencedTimberMan2 = new Thread(startExperiencedTimberMans.cutTimber);
                    Thread ExperiencedTimberMan3 = new Thread(startExperiencedTimberMans.cutTimber);
                    Thread ExperiencedTimberMan4 = new Thread(startExperiencedTimberMans.cutTimber);
                    Thread ExperiencedTimberMan5 = new Thread(startExperiencedTimberMans.cutTimber);

                    TimberMan1.Start(); TimberMan2.Start(); TimberMan3.Start(); TimberMan4.Start(); TimberMan5.Start();
                    ExperiencedTimberMan1.Start(); ExperiencedTimberMan2.Start(); ExperiencedTimberMan3.Start(); ExperiencedTimberMan4.Start(); ExperiencedTimberMan5.Start();

                    TimberMan1.Join(); TimberMan2.Join(); TimberMan3.Join(); TimberMan4.Join(); TimberMan5.Join();
                    ExperiencedTimberMan1.Join(); ExperiencedTimberMan2.Join(); ExperiencedTimberMan3.Join(); ExperiencedTimberMan4.Join(); ExperiencedTimberMan5.Join();

                    TimberCompany.timberMansLogsPerShift.Add(TimberCompany.logsGatherdByTimberMansWithinShift);
                    TimberCompany.experiencedTimberMansLogsPerShift.Add(TimberCompany.logsGatherdByExperiencedTimberMansWithinShift);
                    TimberCompany.logsGatherdByTimberMansWithinShift = 0;
                    TimberCompany.logsGatherdByExperiencedTimberMansWithinShift = 0;

                    if (workShifts < amountOfShifts)
                    {
                        WriteLine("Break Time");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        WriteLine("Work is over");
                    }
                    workShifts++;
                }
                for (int i=1;i<= amountOfShifts; i++)
                {
                    WriteLine("During "+i+" shift TimberMan's obtained: "+ TimberCompany.timberMansLogsPerShift[i-1]+" and Experienced TimberMan's obtained: " + TimberCompany.experiencedTimberMansLogsPerShift[i-1]);
                }
                WriteLine("During the day " + TimberCompany.log + " logs were gatherd.");
                ReadKey();
            }
            catch(Exception ex)
            {
                WriteLine("Something went wrong. Details: "+ex);
                ReadKey();
            }
        }
    }
}
