namespace LastLink.Anticipation.Api.Dtos
{
    public class CreateAnticipationDto
    {
        public Guid CreatorId { get; set; }
        public decimal RequestedAmount { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}