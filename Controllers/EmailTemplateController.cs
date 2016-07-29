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
    public class EmailTemplateController : ApiController
    {
        private EmailRepository rep;

        public EmailTemplateController() {
            this.rep = new EmailRepository();
        }

        [Authorize(Roles = "admin")]
        public EmailTemplate[] Get([FromUri(Name = "isArray")]bool isArray)
        {
            return rep.GetAllEmailTemplates().ToArray();
        }

        public EmailTemplate Get(int Id)
        {
            return rep.GetEmailTemplate(Id);
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Post(EmailTemplate template)
        {
            HttpStatusCode hsc = HttpStatusCode.OK;
            if (template.Id > 0)
            {
                this.rep.SaveEmailTemplate(template);
            }
            else
            {
                int newId = this.rep.CreateTemplate(template);
                template.Id = newId;
                hsc = HttpStatusCode.Created;
            }

            var response = Request.CreateResponse<EmailTemplate>(hsc, template);

            return response;
        }
    }
}
