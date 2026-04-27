using Fitness.Features.Dtos;
using Fitness.Features.Suggestions;
using FitnessCalculationService.Features.WeightGoalActivity.Comands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCalculationService.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class WgaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WgaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddWga([FromBody] AddWGA dto)
        {
            var userId = await _mediator.Send(new WeightGoalActivityAddComand(dto));
            var stats = await _mediator.Send(new CalculateUserFitnessCommand(userId));
            return Ok(new { Success = true, UserId = userId, Stats = stats });
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateWga(Guid userId, [FromBody] AddWGA dto)
        {
            if (userId != dto.UserId)
                return BadRequest("UserId mismatch");

            var id = await _mediator.Send(new WeightGoalActivityUpdateComand(dto));
            var stats = await _mediator.Send(new CalculateUserFitnessCommand(userId));
            return Ok(new { Success = true, UserId = id, Stats = stats });
        }

        [HttpGet("{userId:guid}/suggestions")]
        public async Task<IActionResult> GetSuggestions(Guid userId)
        {
            var suggestions = await _mediator.Send(new GetSuggestionsQuery { UserId = userId });
            return Ok(suggestions);
        }
    }
}