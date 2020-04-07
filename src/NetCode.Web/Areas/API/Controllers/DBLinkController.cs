using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NetCode.Web.Areas.API.Controllers
{
    public class DBLinkController : ApiBaseController
    {
        // GET: api/DBLink
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DBLink/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DBLink
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DBLink/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DBLink/5
        public void Delete(int id)
        {
        }
    }
}
