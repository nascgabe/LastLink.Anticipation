using LastLink.Anticipation.Api.Dtos;
using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LastLink.Anticipation.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AnticipationController : ControllerBase
    {
        private readonly IAnticipationService _service;
        private readonly ILogger<AnticipationController> _logger;

        public AnticipationController(IAnticipationService service, ILogger<AnticipationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAnticipationDto dto)
        {
            _logger.LogInformation("Received Create request for CreatorId={CreatorId}, Amount={Amount}", dto.CreatorId, dto.RequestedAmount);

            try
            {
                var created = await _service.CreateAsync(dto.CreatorId, dto.RequestedAmount, dto.RequestedAt);
                var response = Map(created);

                _logger.LogInformation("Request {RequestId} created successfully", response.Id);
                return CreatedAtAction(nameof(GetById), new { id = response.Id, version = "1.0" }, response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid payload for CreatorId={CreatorId}: {Message}", dto.CreatorId, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Conflict creating request for CreatorId={CreatorId}: {Message}", dto.CreatorId, ex.Message);
                return Conflict(ex.Message);
            }
        }

        [HttpGet("by-creator/{creatorId:guid}")]
        public async Task<IActionResult> ListByCreator(Guid creatorId)
        {
            _logger.LogInformation("Received ListByCreator request for CreatorId={CreatorId}", creatorId);

            var list = await _service.ListByCreatorAsync(creatorId);
            var mapped = list.Select(Map);

            _logger.LogInformation("Returning {Count} requests for CreatorId={CreatorId}", mapped.Count(), creatorId);
            return Ok(mapped);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Received GetById request for RequestId={RequestId}", id);

            try
            {
                var req = await _service.GetByIdAsync(id);
                _logger.LogInformation("Request {RequestId} found", id);
                return Ok(Map(req));
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Request {RequestId} not found", id);
                return NotFound();
            }
        }

        [HttpPatch("{id:guid}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            _logger.LogInformation("Received Approve request for RequestId={RequestId}", id);

            try
            {
                var updated = await _service.ApproveAsync(id);
                _logger.LogInformation("Request {RequestId} approved successfully", id);
                return Ok(Map(updated));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Request {RequestId} not found for approval: {Message}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation approving RequestId={RequestId}: {Message}", id, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id:guid}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            _logger.LogInformation("Received Reject request for RequestId={RequestId}", id);

            try
            {
                var updated = await _service.RejectAsync(id);
                _logger.LogInformation("Request {RequestId} rejected successfully", id);
                return Ok(Map(updated));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Request {RequestId} not found for rejection: {Message}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation rejecting RequestId={RequestId}: {Message}", id, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private static AnticipationResponseDto Map(AnticipationRequest r) => new AnticipationResponseDto
        {
            Id = r.Id,
            CreatorId = r.CreatorId,
            RequestedAmount = r.RequestedAmount,
            NetAmount = r.NetAmount,
            RequestedAt = r.RequestedAt,
            Status = r.Status
        };
    }
}
