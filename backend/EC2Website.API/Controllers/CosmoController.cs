using EC2Website.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace EC2Website.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CosmoController : ControllerBase
    {
            // Instance of context file
            private CosmoDbContext _cosmoContext;

            public CosmoController(CosmoDbContext temp) => _cosmoContext = temp;

        [HttpGet("CosmoPictures")]
        public IEnumerable<Cosmo> GetCosmos()
        {
            return _cosmoContext.Cosmos.ToList();
        }
    }
}
