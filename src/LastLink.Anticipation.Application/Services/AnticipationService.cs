using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Domain.Models;
using LastLink.Anticipation.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace LastLink.Anticipation.Application.Services
{
    public class AnticipationService : IAnticipationService
    {
        private readonly IAnticipationRepository _repository;
        private readonly ILogger<AnticipationService> _logger;

        public AnticipationService(IAnticipationRepository repository, ILogger<AnticipationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<AnticipationRequest> CreateAsync(Guid creatorId, decimal requestedAmount, DateTime requestedAt)
        {
            _logger.LogInformation("Creating anticipation request for CreatorId={CreatorId}, Amount={Amount}", creatorId, requestedAmount);

            var pending = await _repository.GetPendingByCreatorAsync(creatorId);
            if (pending != null)
            {
                _logger.LogWarning("Creator {CreatorId} already has a pending request", creatorId);
                throw new InvalidOperationException("Creator already has a pending request.");
            }

            var request = new AnticipationRequest(creatorId, requestedAmount, requestedAt);
            await _repository.AddAsync(request);

            _logger.LogInformation("Anticipation request {RequestId} created successfully", request.Id);
            return request;
        }

        public async Task<IEnumerable<AnticipationRequest>> ListByCreatorAsync(Guid creatorId)
        {
            _logger.LogInformation("Listing anticipation requests for CreatorId={CreatorId}", creatorId);
            return await _repository.GetByCreatorAsync(creatorId);
        }

        public async Task<AnticipationRequest> ApproveAsync(Guid id)
        {
            _logger.LogInformation("Approving anticipation request {RequestId}", id);

            var req = await _repository.GetByIdAsync(id);
            if (req == null)
            {
                _logger.LogError("Request {RequestId} not found for approval", id);
                throw new KeyNotFoundException("Request not found.");
            }

            req.Approve();
            await _repository.UpdateAsync(req);

            _logger.LogInformation("Request {RequestId} approved successfully", id);
            return req;
        }

        public async Task<AnticipationRequest> RejectAsync(Guid id)
        {
            _logger.LogInformation("Rejecting anticipation request {RequestId}", id);

            var req = await _repository.GetByIdAsync(id);
            if (req == null)
            {
                _logger.LogError("Request {RequestId} not found for rejection", id);
                throw new KeyNotFoundException("Request not found.");
            }

            req.Reject();
            await _repository.UpdateAsync(req);

            _logger.LogInformation("Request {RequestId} rejected successfully", id);
            return req;
        }

        public async Task<AnticipationRequest> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching anticipation request {RequestId}", id);

            var req = await _repository.GetByIdAsync(id);
            if (req == null)
            {
                _logger.LogError("Request {RequestId} not found", id);
                throw new KeyNotFoundException("Request not found.");
            }

            return req;
        }
    }
}