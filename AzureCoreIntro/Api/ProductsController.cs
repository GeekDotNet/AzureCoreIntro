using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureCoreIntro.Api
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        [HttpGet]
        [Route("GetTopCloudProducts")]
        public async Task<IEnumerable<string>> GetProducts()
        {
            List<string> products = new List<string> { "Azure", "AWS", "GCP" };

            return await Task.Run(() => products);
        }
    }
}