using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Need.ApiGateway.Models;
using Need.ApiGateway.Database;
using System;

namespace Need.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToiletController : ControllerBase
    {
        private readonly ILogger<ToiletController> _logger;
        private readonly IRepository<Toilet> _repository;

        public ToiletController(ILogger<ToiletController> logger, IRepository<Toilet> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Toilet>> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var toilet = await _repository.GetAsync(id);

            if (toilet == null)
                return NotFound();

            return Ok(toilet);
        }
    }
}
