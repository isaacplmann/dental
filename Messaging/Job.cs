using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OSUDental.Messaging
{
    public abstract class Job
    {
        public string Title;
        public DateTime ExecutionTime;
        public TimeSpan RepeatEvery;

        public Job(string title, DateTime executionTime)
        {
            this.Title = title;
            this.ExecutionTime = executionTime;
        }
        public Job(string title, DateTime executionTime, TimeSpan repeatEvery)
        {
            this.Title = title;
            this.ExecutionTime = executionTime;
            this.RepeatEvery = repeatEvery;
        }

        public void PreExecute()
        {
            Debug.WriteLine("Executing job at: " + DateTime.Now);
            Debug.WriteLine(this.Title);
            Debug.WriteLine(this.ExecutionTime);
            if (RepeatEvery != null)
            {
                ExecutionTime = ExecutionTime.Add(RepeatEvery);
                lock (CacheMockService._JobQueue)
                {
                    CacheMockService._JobQueue.Add(this);
                }
            }
        }

        public abstract void Execute();
    }
}