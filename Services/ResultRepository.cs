using OSUDental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OSUDental.Services
{
    public class ResultRepository
    {
        private const string CacheKey = "ResultStore";
        public ResultRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var contacts = new Result[]
                    {
                        new Result {
                            Id = 1,
                            TestDate = DateTime.Now.AddDays(-7),
                            EnterDate = DateTime.Now,
                            TestResult = true,
                            EquipId = "A1",
                            Reference = "Some Notes"
                        },
                        new Result {
                            Id = 2,
                            TestDate = DateTime.Now.AddDays(-3),
                            EnterDate = DateTime.Now.AddDays(-5),
                            TestResult = false,
                            EquipId = "B1",
                            Reference = "Here is something else"
                        },
                        new Result {
                            Id = 3,
                            TestDate = DateTime.Now.AddDays(-6),
                            EnterDate = DateTime.Now,
                            TestResult = true,
                            EquipId = "C1",
                            Reference = "Watermelons"
                        },
                        new Result {
                            Id = 4,
                            TestDate = DateTime.Now.AddDays(-8),
                            EnterDate = DateTime.Now.AddDays(-1),
                            TestResult = false,
                            EquipId = "A2",
                            Reference = "Some Other Notes"
                        }
                    };

                    ctx.Cache[CacheKey] = contacts;
                }
            }
        }

        private bool HasId(int Id, Result result)
        {
            return result.Id == Id;
        }

        public Result[] GetAllResults() {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                return (Result[])ctx.Cache[CacheKey];
            }

            return new Result[]
                {
                    new Result
                    {
                        Id = 0,
                        TestDate = DateTime.Now.AddDays(-7),
                        EnterDate = DateTime.Now,
                        TestResult = false,
                        EquipId = "none",
                        Reference = "placeholder"
                    }
                };
        }

        public Result GetResult(int Id) {
            foreach (Result r in GetAllResults()) {
                if (r.Id == Id)
                {
                    return r;
                }
            }
            return null;
        }

        public bool SaveResult(Result result) {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Result[])ctx.Cache[CacheKey]).ToList();
                    if (result.Id == 0)
                    {
                        result.Id = new Random().Next(100, 1000);
                        currentData.Add(result);
                    }
                    else
                    {
                        bool found = false;
                        foreach (Result r in currentData) {
                            if (r.Id == result.Id)
                            {
                                r.TestDate = result.TestDate;
                                r.EnterDate = result.TestDate;
                                r.TestResult = result.TestResult;
                                r.EquipId = result.EquipId;
                                r.Reference = result.Reference;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            currentData.Add(result);
                        }
                    }
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }

        public Result DeleteResult(int resultId)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Result[])ctx.Cache[CacheKey]).ToList();
                    Result result = new Result();
                    foreach (Result r in currentData)
                    {
                        if (r.Id == resultId)
                        {
                            result = r;
                            currentData.Remove(r);
                            break;
                        }
                    }
                    ctx.Cache[CacheKey] = currentData.ToArray();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }

            return null;
        }
    }
}