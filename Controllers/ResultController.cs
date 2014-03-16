using OSUDental.Models;
using OSUDental.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OSUDental.Controllers
{
    public class ResultController : ApiController
    {
        private ResultRepository rep;

        public ResultController() {
            this.rep = new ResultRepository();
        }

        //public int Get(String command)
        //{
        //    if (!command.Equals("count"))
        //    {
        //        return 0;
        //    }
        //    return rep.GetTotalResults(null, null);
        //}

        //public int Get(String command, [FromUri(Name = "startDate")]DateTime? startDate, [FromUri(Name = "endDate")]DateTime? endDate)
        //{
        //    if (!command.Equals("count"))
        //    {
        //        return 0;
        //    }
        //    return rep.GetTotalResults(startDate, endDate);
        //}

        public Result[] Get([FromUri(Name = "isArray")]bool isArray)
        {
            if (AuthenticationHelper.IsAdmin())
            {
                return rep.GetAllResults().ToArray();
            }
            return rep.GetAllResults(AuthenticationHelper.GetClientId()).ToArray();
        }

        [Authorize(Roles = "admin")]
        public Result[] Get(String command, int clientId)
        {
            if (command.Equals("client"))
            {
                return rep.GetAllResults(clientId).ToArray();
            }
            return new Result[0];
        }

        public Result[] Get([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction, [FromUri(Name = "search")]string search)
        {
            if (search != null && search.Equals("null"))
            {
                search = null;
            }
            return rep.GetAllResults(null, null, new PageDetails(page, pageSize, sortColumn, direction), search).ToArray();
        }

        public Result[] Get([FromUri(Name = "startDate")]DateTime? startDate, [FromUri(Name = "endDate")]DateTime? endDate, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction, [FromUri(Name = "search")]string search)
        {
            if (search != null && search.Equals("null"))
            {
                search = null;
            }
            return rep.GetAllResults(startDate, endDate, new PageDetails(page, pageSize, sortColumn, direction), search).ToArray();
        }

        [Authorize(Roles = "admin")]
        public Result[] Get([FromUri(Name = "clientId")]int clientId, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction, [FromUri(Name = "search")]string search)
        {
            if (search != null && search.Equals("null"))
            {
                search = null;
            }
            return rep.GetAllResults(clientId, null, null, new PageDetails(page, pageSize, sortColumn, direction), search).ToArray();
        }

        [Authorize(Roles = "admin")]
        public Result[] Get([FromUri(Name = "clientId")]int clientId, [FromUri(Name = "startDate")]DateTime? startDate, [FromUri(Name = "endDate")]DateTime? endDate, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction, [FromUri(Name = "search")]string search)
        {
            if (search != null && search.Equals("null"))
            {
                search = null;
            }
            return rep.GetAllResults(clientId, startDate, endDate, new PageDetails(page, pageSize, sortColumn, direction), search).ToArray();
        }

        public Result Get(int Id)
        {
            return rep.GetResult(Id);
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Post(Result result)
        {
            HttpStatusCode hsc = HttpStatusCode.OK;
            if (result.Id > 0)
            {
                this.rep.SaveResult(result);
            }
            else
            {
                int newId = this.rep.CreateResult(result);
                result.Id = newId;
                hsc = HttpStatusCode.Created;
            }

            var response = Request.CreateResponse<Result>(hsc, result);

            return response;
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Delete(int Id)
        {
            Result result = this.rep.DeleteResult(Id);
            var response = Request.CreateResponse<Result>(System.Net.HttpStatusCode.OK,result);
            return response;
        }
    }
}
