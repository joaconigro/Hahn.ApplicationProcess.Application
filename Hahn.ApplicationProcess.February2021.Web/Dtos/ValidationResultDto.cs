namespace Hahn.Web.Dtos
{
    public class ValidationResultDto
    {
        public bool IsValid { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
