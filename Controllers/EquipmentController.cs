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
    public class EquipmentController : ApiController
    {
        private EquipmentRepository rep;

        public EquipmentController() {
            this.rep = new EquipmentRepository();
        }

        public Equipment[] Get([FromUri(Name = "isArray")]bool isArray)
        {
            if (AuthenticationHelper.IsAdmin())
            {
                return rep.GetAllEquipment().ToArray();
            }
            return rep.GetAllEquipment(AuthenticationHelper.GetClientId()).ToArray();
        }

        [Authorize(Roles = "admin")]
        public Equipment[] Get(String command, int clientId)
        {
            if(command.Equals("client")) {
                return rep.GetAllEquipment(clientId).ToArray();
            }
            return new Equipment[0];
        }

        public Equipment[] Get([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            
            return rep.GetAllEquipment(new PageDetails(page, pageSize, sortColumn, direction)).ToArray();
        }

        [Authorize(Roles = "admin")]
        public Equipment[] Get([FromUri(Name = "clientId")]int clientId, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllEquipment(clientId, new PageDetails(page, pageSize, sortColumn, direction)).ToArray();
        }

        public Equipment Get(int id)
        {
            return rep.GetEquipment(id);
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Post(Equipment equipment)
        {
            this.rep.SaveEquipment(equipment);

            var response = Request.CreateResponse<Equipment>(System.Net.HttpStatusCode.Created, equipment);

            return response;
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Delete(int Id)
        {
            Equipment equipment = this.rep.DeleteEquipment(Id);
            var response = Request.CreateResponse<Equipment>(System.Net.HttpStatusCode.OK,equipment);
            return response;
        }
    }
}
