using LastLink.Anticipation.Application.Services;
using LastLink.Anticipation.Domain.Models;
using LastLink.Anticipation.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LastLink.Anticipation.Tests.Units
{
    public class AnticipationServiceTests
    {
        private readonly Mock<IAnticipationRepository> _repoMock;
        private readonly AnticipationService _service;

        public AnticipationServiceTests()
        {
            _repoMock = new Mock<IAnticipationRepository>();
            _service = new AnticipationService(_repoMock.Object, NullLogger<AnticipationService>.Instance);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateRequest_WhenNoPendingExists()
        {
            var creatorId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetPendingByCreatorAsync(creatorId))
                     .ReturnsAsync((AnticipationRequest)null);

            var result = await _service.CreateAsync(creatorId, 1000m, DateTime.UtcNow);

            Assert.Equal(creatorId, result.CreatorId);
            Assert.Equal(1000m, result.RequestedAmount);
            Assert.Equal(950m, result.NetAmount);
            Assert.Equal("Pending", result.Status.ToString());
            _repoMock.Verify(r => r.AddAsync(It.IsAny<AnticipationRequest>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenPendingExists()
        {
            var creatorId = Guid.NewGuid();
            var pending = new AnticipationRequest(creatorId, 500m, DateTime.UtcNow);
            _repoMock.Setup(r => r.GetPendingByCreatorAsync(creatorId))
                     .ReturnsAsync(pending);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateAsync(creatorId, 1000m, DateTime.UtcNow));
        }

        [Fact]
        public async Task ApproveAsync_ShouldUpdateStatus_WhenRequestExists()
        {
            var req = new AnticipationRequest(Guid.NewGuid(), 1000m, DateTime.UtcNow);
            _repoMock.Setup(r => r.GetByIdAsync(req.Id)).ReturnsAsync(req);

            var result = await _service.ApproveAsync(req.Id);

            Assert.Equal("Approved", result.Status.ToString());
            _repoMock.Verify(r => r.UpdateAsync(req), Times.Once);
        }

        [Fact]
        public async Task ApproveAsync_ShouldThrow_WhenRequestNotFound()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((AnticipationRequest)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ApproveAsync(id));
        }

        [Fact]
        public async Task RejectAsync_ShouldUpdateStatus_WhenRequestExists()
        {
            var req = new AnticipationRequest(Guid.NewGuid(), 1000m, DateTime.UtcNow);
            _repoMock.Setup(r => r.GetByIdAsync(req.Id)).ReturnsAsync(req);

            var result = await _service.RejectAsync(req.Id);

            Assert.Equal("Rejected", result.Status.ToString());
            _repoMock.Verify(r => r.UpdateAsync(req), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRequest_WhenExists()
        {
            var req = new AnticipationRequest(Guid.NewGuid(), 1000m, DateTime.UtcNow);
            _repoMock.Setup(r => r.GetByIdAsync(req.Id)).ReturnsAsync(req);

            var result = await _service.GetByIdAsync(req.Id);

            Assert.Equal(req.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((AnticipationRequest)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(id));
        }
    }
}