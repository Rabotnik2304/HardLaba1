using System.Globalization;
using System.Text.RegularExpressions;

namespace HardLaba1
{
    public class Reader
    {
        public uint Id;
        public string FullName;
    }
    public class Book
    {
        public uint Id;
        public string Author;
        public string Name;
        public DateTime PublicationDate;
        public uint BookcaseNumber;
        public uint ShelfNumber;
    }
    public class Statistic
    {
        public uint Id;
        public Reader Reader;
        public Book Book;
        public DateTime TakeDate;
        public DateTime ReturnDate;
    }
    public class Program
    {
        static void Main()
        {
            var cultureInfo = new CultureInfo("ru-RU", false);

            Reader reader1 = new Reader{ Id = 1,FullName="Петров Петр Иванович"};
            Reader reader2 = new Reader { Id = 2, FullName = "Иванов Иван Петрович"};

            Book book1 = new Book
            {
                Id = 1,
                Author = "Алесандр Сергеевич Пушкин",
                Name = "Борис Годунов",
                PublicationDate = DateTime.Parse("12.06.1831", cultureInfo),
                BookcaseNumber = 12,
                ShelfNumber = 38
            };
            Book book2 = new Book
            {
                Id = 2,
                Author = "Лев Николаевич Толстой",
                Name = "Война и Мир",
                PublicationDate = DateTime.Parse("16.09.1867", cultureInfo),
                BookcaseNumber = 162,
                ShelfNumber = 56
            };

            Statistic Statistic1 = new Statistic
            {   
                Id=1,
                Reader=reader1,
                Book =book1,
                TakeDate = DateTime.Parse("16.09.2007", cultureInfo),
                ReturnDate = DateTime.Parse("16.10.2007", cultureInfo),
            };
            Statistic Statistic2 = new Statistic
            {
                Id = 2,
                Reader = reader2,
                Book = book2,
                TakeDate = DateTime.Parse("17.11.2009", cultureInfo),
                ReturnDate = DateTime.Parse("19.12.2009", cultureInfo),
            };
        }
    }
}