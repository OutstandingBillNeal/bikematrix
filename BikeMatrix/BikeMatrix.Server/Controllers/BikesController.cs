using BikeMatrix.Data.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UnitsOfWork;

/* 
A note on validation
====================
Jeremy Skinner - the author of FluentValidation - writes
(https://github.com/FluentValidation/FluentValidation/issues/1960)
"We recommend using a manual validation approach with ASP.NET".

In my own experience, we used to be able to have validation run 
as part of the pipeline.  My attempts to make this work in .NET 
Core have been unsuccessful so far.  I'm now adopting Skinner's
reccommendation.
*/

namespace BikeMatrix.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BikesController(
        IMediator mediator
        , IValidator<CreateBikeRequest> createBikeValidator
    ) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IValidator<CreateBikeRequest> _createBikeValidator = createBikeValidator;

        // GET: api/<BikesController>
        [HttpGet(Name = "GetBikes")]
        //public async Task<ActionResult<IEnumerable<Bike>>> Get()
        public async Task<IEnumerable<Bike>> Get()
        {
            var request = new GetBikesRequest();
            var result = await _mediator.Send(request);
            //return new OkObjectResult(result);
            return result;
        }

        // GET api/<BikesController>/5
        [HttpGet("{id}")]
        public string GetById(int id)
        {
            return "value";
        }

        // POST api/<BikesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Bike bike)
        {
            var request = new CreateBikeRequest { Bike = bike };
            var validationResult = await _createBikeValidator.ValidateAsync(request);

            if (validationResult != null && !validationResult.IsValid)
            {
                var message = GetValidationMessage(validationResult);
                return BadRequest(message);
            }

            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = result?.Id }, result);
        }

        // PUT api/<BikesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BikesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private static string GetValidationMessage(ValidationResult validationResult)
        {
            var errorMessages = validationResult
                .Errors
                .Select(err => $"Property: {err.PropertyName}, error: {err.ErrorMessage}");
            return string.Join(' ', errorMessages);
        }

    }
}
