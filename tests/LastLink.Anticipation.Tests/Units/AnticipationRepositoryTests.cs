using LastLink.Anticipation.Domain.Models;
using LastLink.Anticipation.Infrastructure;
using LastLink.Anticipation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace LastLink.Anticipation.Tests.Units
{
    public class AnticipationRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldPersistRequest()
        {
            var ctx = GetDbContext();
            var repo = new AnticipationRepository(ctx, NullLogger<AnticipationRepository>.Instance);

            var req = new AnticipationRequest(Guid.NewGuid(), 1000m, DateTime.UtcNow);
            await repo.AddAsync(req);

            Assert.Equal(1, ctx.Anticipations.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRequest()
        {
            var ctx = GetDbContext();
            var repo = new AnticipationRepository(ctx, NullLogger<AnticipationRepository>.Instance);

            var req = new AnticipationRequest(Guid.NewGuid(), 1000m, DateTime.UtcNow);
            await repo.AddAsync(req);

            var found = await repo.GetByIdAsync(req.Id);
            Assert.NotNull(found);
            Assert.Equal(req.Id, found.Id);
        }

        [Fact]
        public async Task GetByCreatorAsync_ShouldReturnOnlyCreatorRequests()
        {
            var ctx = GetDbContext();
            var repo = new AnticipationRepository(ctx, NullLogger<AnticipationRepository>.Instance);

            var creator1 = Guid.NewGuid();
            var creator2 = Guid.NewGuid();

            await repo.AddAsync(new AnticipationRequest(creator1, 500m, DateTime.UtcNow));
            await repo.AddAsync(new AnticipationRequest(creator2, 600m, DateTime.UtcNow));

            var list = await repo.GetByCreatorAsync(creator1);
            Assert.Single(list);
            Assert.All(list, r => Assert.Equal(creator1, r.CreatorId));
        }

        [Fact]
        public async Task GetPendingByCreatorAsync_ShouldReturnOnlyPending()
        {
            var ctx = GetDbContext();
            var repo = new AnticipationRepository(ctx, NullLogger<AnticipationRepository>.Instance);

            var creator = Guid.NewGuid();
            var pending = new AnticipationRequest(creator, 500m, DateTime.UtcNow);
            var approved = new AnticipationRequest(creator, 600m, DateTime.UtcNow);
            approved.Approve();

            await repo.AddAsync(pending);
            await repo.AddAsync(approved);

            var found = await repo.GetPendingByCreatorAsync(creator);
            Assert.NotNull(found);
            Assert.Equal("Pending", found.Status.ToString());
        }

        [Fact]
        public async Task UpdateAsync_ShouldPersistChanges()
        {
            var ctx = GetDbContext();
            var repo = new AnticipationRepository(ctx, NullLogger<AnticipationRepository>.Instance);

            var req = new AnticipationRequest(Guid.NewGuid(), 500m, DateTime.UtcNow);
            await repo.AddAsync(req);

            req.Approve();
            await repo.UpdateAsync(req);

            var found = await repo.GetByIdAsync(req.Id);
            Assert.Equal("Approved", found.Status.ToString());
        }
    }
}