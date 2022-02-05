using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProcessKiller
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("[Имя процесса] [Время жизни процесса] [Частота проверки (в минутах)]");
                return;
            }
            bool ok = false;
            string processName = args[0];
            int processLifetime;
            int checkPerMinutes;
            
            ok = int.TryParse(args[1], out processLifetime);
            if (!ok)
            {
                Console.WriteLine("Некорректный ввод 2-го аргумента");
                return;
            }
            ok = int.TryParse(args[2], out checkPerMinutes);
            if (!ok)
            {
                Console.WriteLine("Некорректный ввод 3-го аргумента");
                return;
            }

            StartKillLoop(processName, processLifetime, checkPerMinutes);
            while(true) 
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    break;
            }
        }

        static void StartKillLoop(string processName, int lifetime, int checkPerMinutes) 
        {
            Task.Run(async () => {

                while (true)
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    foreach(var item in processes)
                    {

                        if((int)(DateTime.Now - item.StartTime).TotalMinutes >= lifetime)
                        {
                            item.Kill();
                            Console.WriteLine($"Процесс {item.Id} был убит");
                        }
                    }
                    Console.WriteLine($"Всего процессов: {processes.Length}");
                    await Task.Delay(checkPerMinutes * 60 * 1000);
                }
            });
        }
    }
}
