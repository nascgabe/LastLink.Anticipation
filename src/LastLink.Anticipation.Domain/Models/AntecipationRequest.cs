using LastLink.Anticipation.Domain.Enums;


namespace LastLink.Anticipation.Domain.Models
{
    public class AnticipationRequest
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid CreatorId { get; private set; }
        public decimal RequestedAmount { get; private set; }
        public decimal NetAmount { get; private set; }
        public DateTime RequestedAt { get; private set; }
        public AnticipationStatus Status { get; private set; } = AnticipationStatus.Pending;
        private const decimal FeeRate = 0.05m;
        public AnticipationRequest() { }
        public AnticipationRequest(Guid creatorId, decimal requestedAmount, DateTime requestedAt)
        {
            if (requestedAmount <= 100m) throw new ArgumentException("Requested amount must be greater than 100.");
            CreatorId = creatorId;
            RequestedAmount = decimal.Round(requestedAmount, 2);
            RequestedAt = requestedAt;
            NetAmount = CalculateNet(requestedAmount);
        }
        private static decimal CalculateNet(decimal amount)
        {
            var value = amount * (1 - FeeRate);
            return decimal.Round(value, 2);
        }
        public void Approve()
        {
            if (Status != AnticipationStatus.Pending) throw new InvalidOperationException("Only pending requests can be approved.");
            Status = AnticipationStatus.Approved;
        }
        public void Reject()
        {
            if (Status != AnticipationStatus.Pending) throw new InvalidOperationException("Only pending requests can be rejected.");
            Status = AnticipationStatus.Rejected;
        }
    }
}