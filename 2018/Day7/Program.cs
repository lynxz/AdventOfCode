using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        public void FirstStar()
        {
            var prereq = GeneratePreReq();
            var available = prereq.Where(r => r.Value.Count == 0).Select(r => r.Key).OrderBy(v => v);

            var done = new List<string>();
            while (prereq.Count > 0)
            {
                System.Console.WriteLine(string.Join("", available));
                var work = available.First();
                prereq.Remove(work);
                foreach (var req in prereq)
                {
                    req.Value.Remove(work);
                }
                done.Add(work);
            }

            System.Console.WriteLine(string.Join("", done));
        }

        private Dictionary<string, List<string>> GeneratePreReq()
        {
            var data = GetData();
            var prereq = new Dictionary<string, List<string>>();
            foreach (var entry in data)
            {
                var req = entry.Substring(5, 1);
                var task = entry.Substring(36, 1);
                if (!prereq.ContainsKey(task))
                {
                    prereq.Add(task, new List<string>());
                }
                if (!prereq.ContainsKey(req))
                {
                    prereq.Add(req, new List<string>());
                }
                prereq[task].Add(req);
            }

            return prereq;
        }

        public void SecondStar()
        {
            var numberOfWorkers = 5;

            var prereq = GeneratePreReq();
            var available = prereq.Where(r => r.Value.Count == 0).Select(r => r.Key).OrderBy(v => v);
            var workers = new int[numberOfWorkers];
            var currentTask = Enumerable.Repeat(string.Empty, numberOfWorkers).ToArray();
            var totalTime = 0;
            do {
                for (int i = 0; i < workers.Length; i++) {
                    if (workers[i] == 0 && available.Count() > 0) {
                        currentTask[i] = available.First();
                        prereq.Remove(currentTask[i]);
                        workers[i] = (int)(currentTask[i][0] - 'A') + 61;
                    } 
                    if (workers[i] > 0) {
                        workers[i]--;
                    }
                }
                for (int i = 0; i < currentTask.Length; i++) {
                    if (workers[i] == 0 && currentTask[i] != string.Empty) {
                        foreach(var req in prereq) {
                            req.Value.Remove(currentTask[i]);
                        }
                        currentTask[i] = string.Empty;
                    }
                }
                totalTime++;
            } while (workers.Any(t => t > 0) || available.Count() != 0);
            System.Console.WriteLine(totalTime);
        }

        string[] GetData() => File.ReadAllLines("input");

    }
}
