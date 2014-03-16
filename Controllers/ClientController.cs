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

        [Authorize(Roles = "admin")]
        public Client[] Get([FromUri(Name = "isArray")]bool isArray)
        {
            if (isArray)
            {
                if (!AuthenticationHelper.IsAdmin())
                {
                    List<Client> clients = new List<Client>();
                    clients.Add(rep.GetClient());
                    return clients.ToArray();
                }
                else
                {
                    return rep.GetAllClients().ToArray();
                }
            }
            return new List<Client>().ToArray();
        }

        [Authorize(Roles = "admin")]
        public Client[] Get([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction, [FromUri(Name = "search")]string search)
        {
            if (!AuthenticationHelper.IsAdmin())
            {
                List<Client> clients = new List<Client>();
                clients.Add(rep.GetClient());
                return clients.ToArray();
            }
            if (search != null && search.Equals("null"))
            {
                search = null;
            }
            return rep.GetAllClients(page, pageSize, sortColumn, direction, search).ToArray();
        }

        public Client Get()
        {
            if (!AuthenticationHelper.IsAdmin())
            {
                return rep.GetClient();
            }
            return new Client();
        }

        [Authorize(Roles = "admin")]
        public Client Get(int Id)
        {
            if (Id == 0 || !AuthenticationHelper.IsAdmin())
            {
                return rep.GetClient();
            }
            return rep.GetClient(Id);
        }

        public HttpResponseMessage Post(Client client)
        {
            HttpStatusCode hsc = HttpStatusCode.OK;
            if (AuthenticationHelper.IsAdmin() || AuthenticationHelper.GetClientId() == client.Id)
            {
                if (client.Id > 0)
                {
                    this.rep.SaveClient(client);
                }
                else
                {
                    int newId = this.rep.CreateClient(client);
                    client.Id = newId;
                    hsc = HttpStatusCode.Created;
                }
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to edit this client.");
            }

            var response = Request.CreateResponse<Client>(hsc, client);

            return response;
        }

        [Authorize(Roles = "admin")]
        public HttpResponseMessage Delete(int Id)
        {
            Client client = this.rep.DeleteClient(Id);
            var response = Request.CreateResponse<Client>(System.Net.HttpStatusCode.OK, client);
            return response;
        }
    }
}
