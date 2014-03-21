using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;

namespace OSUDental.Messaging
{
    public class CacheMockService
    {
        public const string DummyPageUrl = "http://localhost:8081/KeepAliveCacheMockService";
        private const string DummyCacheItemKey = "CacheMockServiceDummyKey";

        public static ArrayList _JobQueue = new ArrayList();

        /// <summary>
        /// Register a cache entry which expires in 1 minute and gives us a callback.
        /// </summary>
        /// <returns></returns>
        public void RegisterCacheEntry()
        {
            // Prevent duplicate key addition
            if (null != HttpContext.Current.Cache[DummyCacheItemKey]) return;

            HttpContext.Current.Cache.Add(DummyCacheItemKey, "DummyValue", null, DateTime.MaxValue,
                TimeSpan.FromMinutes(1), CacheItemPriority.NotRemovable,
                new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }

        /// <summary>
        /// Callback method which gets invoked whenever the cache entry expires.
        /// We can do our "service" works here.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        public void CacheItemRemovedCallback(
            string key,
            object value,
            CacheItemRemovedReason reason
            )
        {
            Debug.WriteLine("Cache item callback: " + DateTime.Now.ToString());

            // Do the service works
            DoWork();

            // We need to register another cache item which will expire again in one
            // minute. However, as this callback occurs without any HttpContext, we do not
            // have access to HttpContext and thus cannot access the Cache object. The
            // only way we can access HttpContext is when a request is being processed which
            // means a webpage is hit. So, we need to simulate a web page hit and then 
            // add the cache item.
            HitPage();
        }

        /// <summary>
        /// Hits a local webpage in order to add another expiring item in cache
        /// </summary>
        private void HitPage()
        {
            WebClient client = new WebClient();
            client.DownloadData(DummyPageUrl);
        }

        /// <summary>
        /// Asynchronously do the 'service' works
        /// </summary>
        private void DoWork()
        {
            Debug.WriteLine("Begin DoWork...");
            Debug.WriteLine("Running as: " + WindowsIdentity.GetCurrent().Name);

            if(_JobQueue.Count==0)
                EnqueueJob(new EmailAlertJob());
            // Reorder Test Strips - 11 months from last subscribe date (harder - 3 strips left)
            // Send in test strips after 14 days of non-activity (52 strip subscribers)
            // Failed Test Alert
            ExecuteQueuedJobs();

            Debug.WriteLine("End DoWork...");
        }

        public void EnqueueJob(Job j)
        {
            lock (_JobQueue)
            {
                _JobQueue.Add(j);
            }
        }

        /// <summary>
        /// Execute jobs that are queued for execution at a specific time
        /// </summary>
        private void ExecuteQueuedJobs()
        {
            ArrayList jobs = new ArrayList();

            // Collect which jobs are overdue
            foreach (Job job in _JobQueue)
            {
                if (job.ExecutionTime <= DateTime.Now)
                    jobs.Add(job);
            }

            // Eecute the jobs that are overdue
            foreach (Job job in jobs)
            {
                lock (_JobQueue)
                {
                    _JobQueue.Remove(job);
                }

                job.Execute();
            }
        }
    }
}