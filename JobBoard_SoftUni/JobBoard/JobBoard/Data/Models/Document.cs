namespace JobBoard.Data.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public byte[]? PdfFileData { get; set; }
        public string ContentType { get; set; }
    }
}
