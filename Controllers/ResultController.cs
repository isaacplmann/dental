using OSUDental.Models;
using OSUDental.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OSUDental.Controllers
{
    public class ResultController : ApiController
    {
        private ResultRepository rep;

        public ResultController() {
            this.rep = new ResultRepository();
        }

        public int Get(String command)
        {
            if (!command.Equals("count"))
            {
                return 0;
            }
            return rep.GetTotalResults(null, null);
        }

        public int Get(String command, [FromUri(Name = "startDate")]DateTime? startDate, [FromUri(Name = "endDate")]DateTime? endDate)
        {
            if (!command.Equals("count"))
            {
                return 0;
            }
            return rep.GetTotalResults(startDate, endDate);
        }

        public Result[] Get()
        {
            return rep.GetAllResults().ToArray();
        }

        public Result[] Get([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllResults(null, null, page, pageSize, sortColumn, direction).ToArray();
        }

        public Result[] Get([FromUri(Name = "startDate")]DateTime? startDate, [FromUri(Name = "endDate")]DateTime? endDate, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllResults(startDate, endDate, page, pageSize, sortColumn, direction).ToArray();
        }

        public Result[] Get(int clientId)
        {
            return rep.GetAllResults(clientId).ToArray();
        }

        public Result[]  ([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllResults(null, null, page, pageSize, sortColumn, direction).ToArray();
        }

        public Result[] Get([FromUri(Name = "startDate")]DateTime? startDate, [FromUri(Name = "endDate")]DateTime? endDate, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllResults(startDate, endDate, page, pageSize, sortColumn, direction).ToArray();
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
