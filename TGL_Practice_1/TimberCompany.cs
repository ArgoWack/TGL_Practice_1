using System;
using static System.Console;
using System.Threading;
using System.Collections.Generic;

namespace TGL_Practice_1
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
        protected internal class TimberMan : Worker, IWork
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
                                gatherdLogs += chances * 2;
                                break;
                            }
                    }
                    logsGatherdByTimberMansWithinShift += gatherdLogs;
                    log += gatherdLogs;
                    WriteLine("On Thread: " + threadSpecificData + " was gatherd: " + gatherdLogs + " of wood by TimberMan which brings it to total of: " + log + " logs");
                }
            }
        }
        protected internal sealed class Experienced_TimberMan : TimberMan, IWork
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
                    WriteLine("On Thread: " + threadSpecificData + " was gatherd: " + gatherdLogs + " of wood by Experienced TimberMan which brings it to total of: " + log + " logs");
                }
            }
        }
    }
}
