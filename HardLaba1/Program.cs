using System.Collections.Generic;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using static System.Reflection.Metadata.BlobBuilder;

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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
                       
            try
            {
                List<Reader> readers = ReadersInitialization();
                List<Book> books = BooksInitialization();
                List<Statistic> statistics = StatisticsInitialization(books, readers);

                int maxLenAuthor = MaxLenAuthorInitialization(books);

                int maxLenNameBook = MaxLenNameBookInitialization(books);

                int maxLenNameReader = MaxLenNameReaderInitialization(readers, books, statistics);

                HeadingInitialization(maxLenAuthor, maxLenNameBook, maxLenNameReader);
                TableInitialization(books, statistics, maxLenAuthor, maxLenNameBook, maxLenNameReader);
            }
            catch (ArgumentException ex)
            {
                Console.Clear();
                Console.Write("Ошибка:");
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static void TableInitialization(List<Book> books, List<Statistic> statistics, int maxLenAuthor, int maxLenNameBook, int maxLenNameReader)
        {
            foreach (Book book in books)
            {
                Console.Write("| ");
                Console.Write(book.Author.PadRight(maxLenAuthor));
                Console.Write(" | ");

                Console.Write(book.Name.PadRight(maxLenNameBook));
                Console.Write(" | ");

                string readerName = "";
                DateTime takeDate = DateTime.MinValue;

                foreach (Statistic statistic in statistics)
                {
                    if (statistic.Book.Id == book.Id)
                    {
                        readerName = statistic.Reader.FullName;
                        takeDate = statistic.TakeDate;
                    }
                }

                Console.Write(readerName.PadRight(maxLenNameReader));
                Console.Write(" | ");

                if (takeDate != DateTime.MinValue)
                {
                    Console.Write(takeDate.ToShortDateString());
                }
                else
                {
                    Console.Write("".PadRight(10));
                }

                Console.WriteLine(" |");
            }
        }

        private static void HeadingInitialization(int maxLenAuthor, int maxLenNameBook, int maxLenNameReader)
        {
            Console.Write("| ");
            Console.Write("Автор".PadRight(maxLenAuthor));
            Console.Write(" | ");

            Console.Write("Название".PadRight(maxLenNameBook));
            Console.Write(" | ");

            Console.Write("Читает".PadRight(maxLenNameReader));
            Console.Write(" | ");

            Console.Write("Взял".PadRight(10));
            Console.WriteLine(" |");

            Console.Write("| ");
            Console.Write("".PadRight(maxLenAuthor, '-'));
            Console.Write(" | ");

            Console.Write("".PadRight(maxLenNameBook, '-'));
            Console.Write(" | ");

            Console.Write("".PadRight(maxLenNameReader, '-'));
            Console.Write(" | ");

            Console.Write("".PadRight(10, '-'));
            Console.WriteLine(" |");
        }

        private static int MaxLenNameReaderInitialization(List<Reader> readers, List<Book> books , List<Statistic> statistics)
        {
            int maxLenNameReader = 0;

            foreach (Book book in books)
            {
                foreach (Statistic statistic in statistics)
                {
                    if (statistic.Book.Id == book.Id)
                    {
                        maxLenNameReader = Math.Max(maxLenNameReader, statistic.Reader.FullName.Length);
                        break;
                    }
                }
            }

            return maxLenNameReader;
        }

        private static int MaxLenAuthorInitialization(List<Book> books)
        {
            int maxLenAuthor = 0;
            foreach (Book book in books)
            {
                maxLenAuthor = Math.Max(maxLenAuthor, book.Author.Length);
            }
            return maxLenAuthor;
        }
        private static int MaxLenNameBookInitialization(List<Book> books)
        {
            int maxLenNameBook = 0;
            foreach (Book book in books)
            {
                
                maxLenNameBook = Math.Max(maxLenNameBook, book.Name.Length);
            }
            return maxLenNameBook;
        }

        private static List<Reader> ReadersInitialization()
        {
            List < Reader > readers = new List < Reader >();
            string[] allLinesReader = File.ReadAllLines("Reader.csv");
            for (int i = 0; i < allLinesReader.Length; i++)
            {
                string[] elementsOfLine = allLinesReader[i].Split(";");
                
                if (elementsOfLine.Length > 2)
                {
                    throw new ArgumentException($"В файле Reader.csv в строке {i + 1} столбцов больше чем 2");
                }
                if (uint.TryParse(elementsOfLine[0], out uint id))
                {
                    id = id;
                }
                else
                {
                    throw new ArgumentException($"В файле Reader.csv в строке {i + 1} в столбце 1 записаны некорректные данные");
                }

                readers.Add(new Reader { Id = id, FullName = elementsOfLine[1] });
            }
            return readers;
        }
        private static List<Book> BooksInitialization()
        {
            List<Book> books = new List<Book>();
            string[] allLinesBook = File.ReadAllLines("Book.csv");
            
            for (int i = 0; i < allLinesBook.Length; i++)
            {
                string[] elementsOfLine = allLinesBook[i].Split(";");

                uint id, bookcaseNumber, shelfNumber;
                DateTime publicationDate;
                BooksLineInitialization(i, elementsOfLine, out id, out publicationDate, out bookcaseNumber, out shelfNumber);

                books.Add(new Book
                {
                    Id = id,
                    Author = elementsOfLine[1],
                    Name = elementsOfLine[2],
                    PublicationDate = publicationDate,
                    BookcaseNumber = bookcaseNumber,
                    ShelfNumber = shelfNumber
                });
            }
            return books;
        }

        private static void BooksLineInitialization(int i, string[] elementsOfLine, out uint id, out DateTime publicationDate, out uint bookcaseNumber, out uint shelfNumber)
        {
            if (elementsOfLine.Length > 6)
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} столбцов больше чем 6");
            }

            if (uint.TryParse(elementsOfLine[0], out id))
            {
                id = id;
            }
            else
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} в столбце 1 записаны некорректные данные");
            }

            if (DateTime.TryParse(elementsOfLine[3], out publicationDate))
            {
                publicationDate = publicationDate;
            }
            else
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} в столбце 4 записаны некорректные данные");
            }

            if (uint.TryParse(elementsOfLine[4], out bookcaseNumber))
            {
                bookcaseNumber = bookcaseNumber;
            }
            else
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} в столбце 5 записаны некорректные данные");
            }

            if (uint.TryParse(elementsOfLine[5], out shelfNumber))
            {
                shelfNumber = shelfNumber;
            }
            else
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} в столбце 6 записаны некорректные данные");
            }
        }

        private static List<Statistic> StatisticsInitialization(List<Book> books, List<Reader> readers)
        {
            
            List<Statistic> statistics = new List<Statistic>();
            string[] allLinesStatistics = File.ReadAllLines("Statistics.csv");

            for (int i = 0; i < allLinesStatistics.Length; i++)
            {
                string[] elementsOfLine = allLinesStatistics[i].Split(";");

                uint id, readerId, bookId;
                DateTime takeDate, returnDate;
                StatisticsLineInitialization(i, elementsOfLine, out id, out readerId, out bookId, out takeDate, out returnDate);

                Reader readerStatistics = StatisticsReaderInitialization(readers, i, readerId);

                Book bookStatistics = StatisticsBookInitialization(books, i, bookId);

                statistics.Add(new Statistic
                {
                    Id = id,
                    Reader = readerStatistics,
                    Book = bookStatistics,
                    TakeDate = takeDate,
                    ReturnDate = returnDate,
                });

            }
            return statistics;
        }

        private static void StatisticsLineInitialization(int i, string[] elementsOfLine, out uint id, out uint readerId, out uint bookId, out DateTime takeDate, out DateTime returnDate)
        {
            if (elementsOfLine.Length > 5)
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} столбцов больше чем 5");
            }

            if (uint.TryParse(elementsOfLine[0], out id))
            {
                id = id;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 1 записаны некорректные данные");
            }

            if (uint.TryParse(elementsOfLine[1], out readerId))
            {
                readerId = readerId;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 2 записаны некорректные данные");
            }

            if (uint.TryParse(elementsOfLine[2], out bookId))
            {
                bookId = bookId;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 3 записаны некорректные данные");
            }

            if (DateTime.TryParse(elementsOfLine[3], out takeDate))
            {
                takeDate = takeDate;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 4 записаны некорректные данные");
            }

            if (DateTime.TryParse(elementsOfLine[4], out returnDate))
            {
                returnDate = returnDate;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 5 записаны некорректные данные");
            }

            if (takeDate > returnDate)
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} читатель вернул книгу до того как её взять");
            }
        }

        private static Reader StatisticsReaderInitialization(List<Reader> readers, int i, uint readerId)
        {
            Reader readerStatistics = null;
            bool readerFlag = false;
            foreach (Reader reader in readers)
            {
                if (reader.Id == readerId)
                {
                    readerStatistics = reader;
                    readerFlag = true;
                    break;
                }
            }

            if (!readerFlag)
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 2 введено id несуществующего читателя");
            }

            return readerStatistics;
        }

        private static Book StatisticsBookInitialization(List<Book> books, int i, uint bookId)
        {
            Book bookStatistics = null;
            bool bookFlag = false;
            foreach (Book book in books)
            {
                if (book.Id == bookId)
                {
                    bookStatistics = book;
                    bookFlag = true;
                    break;
                }
            }

            if (!bookFlag)
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 3 введено id несуществующей книги");
            }

            return bookStatistics;
        }
    }
}