using OSUDental.Models;
using OSUDental.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OSUDental.Controllers
{
    public class ResultController : ApiController
    {
        private ResultRepository rep;

        public ResultController() {
            this.rep = new ResultRepository();
        }

        public Result[] Get()
        {
            return rep.GetAllResults();
        }

        public Result Get(int Id)
        {
            return rep.GetResult(Id);
        }

        public HttpResponseMessage Post(Result result)
        {
            this.rep.SaveResult(result);

            var response = Request.CreateResponse<Result>(System.Net.HttpStatusCode.Created, result);

            return response;
        }

        public HttpResponseMessage Delete(int Id)
        {
            Result result = this.rep.DeleteResult(Id);
            var response = Request.CreateResponse<Result>(System.Net.HttpStatusCode.OK,result);
            return response;
        }
    }
}
