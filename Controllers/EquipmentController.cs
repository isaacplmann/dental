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

        public Equipment[] Get([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction, [FromUri(Name = "search")]string search)
        {
            if (search != null && search.Equals("null"))
            {
                search = null;
            }
            return rep.GetAllEquipment(new PageDetails(page, pageSize, sortColumn, direction), search).ToArray();
        }

        [Authorize(Roles = "admin")]
        public Equipment[] Get([FromUri(Name = "clientId")]int clientId, [FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction, [FromUri(Name = "search")]string search)
        {
            if (search != null && search.Equals("null"))
            {
                search = null;
            }
            return rep.GetAllEquipment(clientId, new PageDetails(page, pageSize, sortColumn, direction), search).ToArray();
        }

        public Equipment Get(int id)
        {
            return rep.GetEquipment(id);
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Post(Equipment equipment)
        {
            HttpStatusCode hsc = HttpStatusCode.OK;
            if (equipment.Id > 0)
            {
                this.rep.SaveEquipment(equipment);
            }
            else
            {
                int newId = this.rep.CreateEquipment(equipment);
                equipment.Id = newId;
                hsc = HttpStatusCode.Created;
            }

            var response = Request.CreateResponse<Equipment>(hsc, equipment);

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
