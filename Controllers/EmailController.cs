using OSUDental.Messaging;
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
    public class EmailController : ApiController
    {
        private EmailRepository rep;

        public EmailController() {
            this.rep = new EmailRepository();
        }

        public EmailTracking[] Get([FromUri(Name = "isArray")]bool isArray)
        {
            if (AuthenticationHelper.IsAdmin())
            {
                return rep.GetAllEmails().ToArray();
            }
            return rep.GetAllEmails(AuthenticationHelper.GetClientId()).ToArray();
        }

        [Authorize(Roles = "admin")]
        public EmailTracking[] Get(String command, int clientId)
        {
            if (command.Equals("client"))
            {
                return rep.GetAllEmails(clientId).ToArray();
            }
            return new EmailTracking[0];
        }

        public EmailTracking[] Get([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllEmails(null, null, new PageDetails(page, pageSize, sortColumn, direction)).ToArray();
        }

        public EmailTracking[] Get([FromUri(Name = "startDate")]DateTime? startDate, [FromUri(Name = "endDate")]DateTime? endDate, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllEmails(startDate, endDate, new PageDetails(page, pageSize, sortColumn, direction)).ToArray();
        }

        [Authorize(Roles = "admin")]
        public EmailTracking[] Get([FromUri(Name = "clientId")]int clientId, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllEmails(clientId, null, null, new PageDetails(page, pageSize, sortColumn, direction)).ToArray();
        }

        [Authorize(Roles = "admin")]
        public EmailTracking[] Get([FromUri(Name = "clientId")]int clientId, [FromUri(Name = "startDate")]DateTime? startDate, [FromUri(Name = "endDate")]DateTime? endDate, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllEmails(clientId, startDate, endDate, new PageDetails(page, pageSize, sortColumn, direction)).ToArray();
        }

        public EmailTracking Get(int Id)
        {
            return rep.GetEmailTracking(Id);
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Post(EmailTracking email)
        {
            HttpStatusCode hsc = HttpStatusCode.OK;
            if (email.Id > 0)
            {
                // unimplemented
            }
            else
            {
                EmailAlert alert = new EmailAlert();
                int newId = alert.Send(email.EmailTypeId, email.ClientId);
                // call EmailAlert.Send(type,client)
                //int newId = this.rep.LogEmail(email); // logging will be done from EmailAlert
                email.Id = newId;
                hsc = HttpStatusCode.Created;
            }

            var response = Request.CreateResponse<EmailTracking>(hsc, email);

            return response;
        }
    }
}
