namespace Mock_Project_Net03.Common.Payloads.Responses
{
    public class PermissionErrorResult
    {
        public bool Success => false;
        public Dictionary<string, Dictionary<string, List<string>>> Errors { get; set; }
    }
}
