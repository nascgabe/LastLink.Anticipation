using LastLink.Anticipation.Domain.Enums;
using LastLink.Anticipation.Domain.Models;
using LastLink.Anticipation.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LastLink.Anticipation.Infrastructure.Repositories
{
    public class AnticipationRepository : IAnticipationRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AnticipationRepository> _logger;

        public AnticipationRepository(AppDbContext context, ILogger<AnticipationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(AnticipationRequest request)
        {
            _logger.LogDebug("Adding anticipation request {RequestId}", request.Id);
            _context.Anticipations.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<AnticipationRequest> GetByIdAsync(Guid id)
        {
            _logger.LogDebug("Fetching anticipation request {RequestId}", id);
            return await _context.Anticipations.FindAsync(id);
        }

        public async Task<IEnumerable<AnticipationRequest>> GetByCreatorAsync(Guid creatorId)
        {
            _logger.LogDebug("Fetching anticipation requests for CreatorId={CreatorId}", creatorId);
            return await _context.Anticipations.Where(x => x.CreatorId == creatorId).ToListAsync();
        }

        public async Task<AnticipationRequest> GetPendingByCreatorAsync(Guid creatorId)
        {
            _logger.LogDebug("Fetching pending anticipation request for CreatorId={CreatorId}", creatorId);
            return await _context.Anticipations.FirstOrDefaultAsync(x => x.CreatorId == creatorId && x.Status == AnticipationStatus.Pending);
        }

        public async Task UpdateAsync(AnticipationRequest request)
        {
            _logger.LogDebug("Updating anticipation request {RequestId}", request.Id);
            _context.Anticipations.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}