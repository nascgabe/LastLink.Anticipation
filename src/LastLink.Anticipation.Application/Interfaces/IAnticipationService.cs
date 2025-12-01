using LastLink.Anticipation.Domain.Models;

namespace LastLink.Anticipation.Application.Interfaces
{
    public interface IAnticipationService
    {
        Task<AnticipationRequest> CreateAsync(Guid creatorId, decimal requestedAmount, DateTime requestedAt);
        Task<IEnumerable<AnticipationRequest>> ListByCreatorAsync(Guid creatorId);
        Task<AnticipationRequest> ApproveAsync(Guid id);
        Task<AnticipationRequest> RejectAsync(Guid id);
        Task<AnticipationRequest> GetByIdAsync(Guid id);
    }
}