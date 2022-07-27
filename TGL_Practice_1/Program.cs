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
