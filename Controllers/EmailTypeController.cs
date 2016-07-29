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
    public class EmailTypeController : ApiController
    {
        private EmailRepository rep;

        public EmailTypeController() {
            this.rep = new EmailRepository();
        }

        [Authorize(Roles = "admin")]
        public EmailType[] Get([FromUri(Name = "isArray")]bool isArray)
        {
            return rep.GetAllEmailTypes().ToArray();
        }
    }
}
