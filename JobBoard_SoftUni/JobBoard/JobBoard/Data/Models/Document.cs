namespace JobBoard.Data.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public byte[] Content { get; set; }
        public DateTime DateUploaded { get; set; }
        
        public int ApplicationId { get; set; }
        public Applications Applications { get; set; }
    }
}
