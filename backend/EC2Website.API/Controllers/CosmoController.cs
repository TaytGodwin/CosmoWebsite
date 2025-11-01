using EC2Website.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EC2Website.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PicturesController : ControllerBase
    {
        private readonly CosmoDbContext _context;

        public PicturesController(CosmoDbContext context)
        {
            _context = context;
        }

        // GET api/pictures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picture>>> GetPictures()
        {
            var pictures = await _context.Pictures.AsNoTracking().ToListAsync();
            return Ok(pictures);
        }
    }
}
