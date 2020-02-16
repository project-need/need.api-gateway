using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Need.ApiGateway.Models;
using Need.ApiGateway.Database;
using System;

namespace Need.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToiletController : ControllerBase
    {
        private readonly IRepository<Toilet> _repository;

        public ToiletController(IRepository<Toilet> repository)
        {
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post(Toilet toilet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var id = await _repository.AddAsync(toilet);
            
            return CreatedAtRoute("Get", new { id = id }, toilet);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> Put(string id, Toilet toilet)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.UpdateAsync(id, toilet);

            return AcceptedAtRoute("Get", new { id = id }, toilet);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
