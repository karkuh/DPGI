#nullable disable

namespace Lab5.Data
{
    public partial class Book
    {
        public string Isbn { get; set; }
        public string Authors { get; set; }
        public int? PublisherCode { get; set; }
        public int? PublicationYear { get; set; }

        public virtual Publisher PublisherCodeNavigation { get; set; }
    }
}