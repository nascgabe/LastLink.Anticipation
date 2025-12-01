using LastLink.Anticipation.Domain.Models;

namespace LastLink.Anticipation.Infrastructure.Interfaces
{
    public interface IAnticipationRepository
    {
        Task AddAsync(AnticipationRequest request);
        Task<AnticipationRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<AnticipationRequest>> GetByCreatorAsync(Guid creatorId);
        Task<AnticipationRequest> GetPendingByCreatorAsync(Guid creatorId);
        Task UpdateAsync(AnticipationRequest request);
    }
}