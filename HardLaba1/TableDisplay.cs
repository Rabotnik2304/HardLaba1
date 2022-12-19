using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace HardLaba1
{
    internal class TableDisplay
    {   
        public static void TableDisplayStart(List<Book> books, List<Statistic> statistics, List<Reader> readers)
        {
            int maxLenAuthor = FindMaxLenAuthor(books);

            int maxLenNameBook = FindMaxLenNameBook(books);

            // находим максимальную длину имени читателя, который взял книгу
            int maxLenNameReader = FindMaxLenNameReader(readers, books, statistics);

            ReturnHeading(maxLenAuthor, maxLenNameBook, maxLenNameReader);
            ReturnTable(books, statistics, maxLenAuthor, maxLenNameBook, maxLenNameReader);
        }

        private static void ReturnTable(List<Book> books, List<Statistic> statistics, int maxLenAuthor, int maxLenNameBook, int maxLenNameReader)
        {
            foreach (Book book in books)
            {
                Console.Write("| ");
                Console.Write(book.Author.PadRight(maxLenAuthor));
                Console.Write(" | ");

                Console.Write(book.Name.PadRight(maxLenNameBook));
                Console.Write(" | ");

                // проверяем - взял ли кто-то эту книгу и если да, записываем его имя и когда он ёё взял
                string readerName = "";
                DateTime takeDate = DateTime.MinValue;
                foreach (Statistic statistic in statistics)
                {
                    if (statistic.Book.Id == book.Id)
                    {
                        readerName = statistic.Reader.FullName;
                        takeDate = statistic.TakeDate;
                        break;
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
                    Console.Write(new string(' ', 10));
                }

                Console.WriteLine(" |");
            }
        }

        private static void ReturnHeading(int maxLenAuthor, int maxLenNameBook, int maxLenNameReader)
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
            Console.Write(new string('-', maxLenAuthor));
            Console.Write(" | ");

            Console.Write(new string('-', maxLenNameBook));
            Console.Write(" | ");

            Console.Write(new string('-', maxLenNameReader));
            Console.Write(" | ");

            Console.Write("".PadRight(10, '-'));
            Console.WriteLine(" |");
        }

        private static int FindMaxLenNameReader(List<Reader> readers, List<Book> books, List<Statistic> statistics)
        {
            int maxLenNameReader = 0;

            foreach (Book book in books)
            {
                foreach (Statistic statistic in statistics)
                {
                    if (statistic.Book.Id == book.Id)
                    {
                        maxLenNameReader = Math.Max(maxLenNameReader, statistic.Reader.FullName.Length);
                    }
                }
            }

            return maxLenNameReader;
        }

        private static int FindMaxLenAuthor(List<Book> books)
        {
            int maxLenAuthor = 0;
            foreach (Book book in books)
            {
                maxLenAuthor = Math.Max(maxLenAuthor, book.Author.Length);
            }
            return maxLenAuthor;
        }
        private static int FindMaxLenNameBook(List<Book> books)
        {
            int maxLenNameBook = 0;
            foreach (Book book in books)
            {
                maxLenNameBook = Math.Max(maxLenNameBook, book.Name.Length);
            }
            return maxLenNameBook;
        }
    }
}
