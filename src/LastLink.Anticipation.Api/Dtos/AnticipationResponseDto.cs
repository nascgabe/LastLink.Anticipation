using LastLink.Anticipation.Domain.Enums;

namespace LastLink.Anticipation.Api.Dtos
{
    public class AnticipationResponseDto
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime RequestedAt { get; set; }
        public AnticipationStatus Status { get; set; }
    }
}