using LastLink.Anticipation.Domain.Models;

namespace LastLink.Anticipation.Tests.Units
{
    public class AnticipationRequestTests
    {
        [Fact]
        public void Constructor_ShouldSetValues_WhenValidAmount()
        {
            var creatorId = Guid.NewGuid();
            var req = new AnticipationRequest(creatorId, 1000m, DateTime.UtcNow);

            Assert.Equal(creatorId, req.CreatorId);
            Assert.Equal(1000m, req.RequestedAmount);
            Assert.Equal(950m, req.NetAmount);
            Assert.Equal("Pending", req.Status.ToString());
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenAmountLessOrEqual100()
        {
            var creatorId = Guid.NewGuid();
            Assert.Throws<ArgumentException>(() => new AnticipationRequest(creatorId, 100m, DateTime.UtcNow));
        }

        [Fact]
        public void Approve_ShouldChangeStatus_WhenPending()
        {
            var req = new AnticipationRequest(Guid.NewGuid(), 500m, DateTime.UtcNow);
            req.Approve();
            Assert.Equal("Approved", req.Status.ToString());
        }

        [Fact]
        public void Approve_ShouldThrow_WhenNotPending()
        {
            var req = new AnticipationRequest(Guid.NewGuid(), 500m, DateTime.UtcNow);
            req.Approve();
            Assert.Throws<InvalidOperationException>(() => req.Approve());
        }

        [Fact]
        public void Reject_ShouldChangeStatus_WhenPending()
        {
            var req = new AnticipationRequest(Guid.NewGuid(), 500m, DateTime.UtcNow);
            req.Reject();
            Assert.Equal("Rejected", req.Status.ToString());
        }

        [Fact]
        public void Reject_ShouldThrow_WhenNotPending()
        {
            var req = new AnticipationRequest(Guid.NewGuid(), 500m, DateTime.UtcNow);
            req.Reject();
            Assert.Throws<InvalidOperationException>(() => req.Reject());
        }
    }
}
