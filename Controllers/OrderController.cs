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
    public class OrderController : ApiController
    {
        private OrderRepository rep;

        public OrderController() {
            this.rep = new OrderRepository();
        }

        public Order[] Get([FromUri(Name = "isArray")]bool isArray)
        {
            if (AuthenticationHelper.IsAdmin())
            {
                return rep.GetAllOrders().ToArray();
            }
            return rep.GetAllOrders(AuthenticationHelper.GetClientId()).ToArray();
        }

        public Order[] Get(String command,int clientId)
        {
            if(command.Equals("client")) {
                return rep.GetAllOrders(clientId).ToArray();
            }
            return new Order[0];
        }

        public Order[] Get([FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            
            return rep.GetAllOrders(new PageDetails(page, pageSize, sortColumn, direction)).ToArray();
        }

        public Order[] Get([FromUri(Name="clientId")]int clientId,[FromUri(Name = "page")]int page, [FromUri(Name = "pageSize")]int pageSize, [FromUri(Name = "sortColumn")]string sortColumn, [FromUri(Name = "direction")]string direction)
        {
            return rep.GetAllOrders(clientId, new PageDetails(page, pageSize, sortColumn, direction)).ToArray();
        }

        public Order Get(int id)
        {
            return rep.GetOrder(id);
        }

        public HttpResponseMessage Post(Order order)
        {
            this.rep.SaveOrder(order);

            var response = Request.CreateResponse<Order>(System.Net.HttpStatusCode.Created, order);

            return response;
        }

        public HttpResponseMessage Delete(int Id)
        {
            Order order = this.rep.DeleteOrder(Id);
            var response = Request.CreateResponse<Order>(System.Net.HttpStatusCode.OK,order);
            return response;
        }
    }
}
