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
    public class ClientController : ApiController
    {
        private ClientRepository rep;

        public ClientController() {
            this.rep = new ClientRepository();
        }

        public Client[] Get()
        {
            return rep.GetAllClients().ToArray();
        }

        public Client[] Get([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllClients(page, pageSize, sortColumn, direction).ToArray();
        }

        public Client Get(int Id)
        {
            return rep.GetClient(Id);
        }

        public HttpResponseMessage Post(Client client)
        {
            this.rep.SaveClient(client);

            var response = Request.CreateResponse<Client>(System.Net.HttpStatusCode.Created, client);

            return response;
        }

        public HttpResponseMessage Delete(int Id)
        {
            Client client = this.rep.DeleteClient(Id);
            var response = Request.CreateResponse<Client>(System.Net.HttpStatusCode.OK,client);
            return response;
        }
    }
}
