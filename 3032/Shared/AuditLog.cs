namespace CampaignManagementTool.Shared;

public class AuditLog
{
    public Guid Id { get; set; }
    public UserInfo AddedBy { get; set; }
    public string CampaignCode { get; set; }
    public List<UpdateValues> Updates { get; set; }
    public DateTime AddedDate { get; set; }

    public class UpdateValues
    {
        public string FieldName { get; set; }
        public string? ValueBefore { get; set; }
        public string? ValueAfter { get; set; }
    }
}