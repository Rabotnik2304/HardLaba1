using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardLaba1
{
    internal class TableReader
    {
        public static List<Book> ReadBooks(string path)
        {
            List<Book> books = new List<Book>();
            string[] allLinesBook = File.ReadAllLines(path);

            for (int i = 0; i < allLinesBook.Length; i++)
            {
                string[] elementsOfLine = allLinesBook[i].Split(";");

                // парсим данные из одной строчки файла Book.csv
                uint id, bookcaseNumber, shelfNumber;
                DateTime publicationDate;
                ReadBooksLine(i, elementsOfLine, out id, out publicationDate, out bookcaseNumber, out shelfNumber);

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

        private static void ReadBooksLine(int i, string[] elementsOfLine, out uint id, out DateTime publicationDate, out uint bookcaseNumber, out uint shelfNumber)
        {
            СheckBooksLineLength(i, elementsOfLine);

            id = ReadBookId(i, elementsOfLine);
            publicationDate = ReadBookPublicationDate(i, elementsOfLine);
            bookcaseNumber = ReadBookBookcaseNumber(i, elementsOfLine);
            shelfNumber = ReadBookShelfNumber(i, elementsOfLine);
        }

        private static uint ReadBookShelfNumber(int i, string[] elementsOfLine)
        {
            uint shelfNumber;
            if (uint.TryParse(elementsOfLine[5], out shelfNumber))
            {
                shelfNumber = shelfNumber;
            }
            else
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} в столбце 6 записаны некорректные данные");
            }

            return shelfNumber;
        }

        private static uint ReadBookBookcaseNumber(int i, string[] elementsOfLine)
        {
            uint bookcaseNumber;
            if (uint.TryParse(elementsOfLine[4], out bookcaseNumber))
            {
                bookcaseNumber = bookcaseNumber;
            }
            else
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} в столбце 5 записаны некорректные данные");
            }

            return bookcaseNumber;
        }

        private static DateTime ReadBookPublicationDate(int i, string[] elementsOfLine)
        {
            DateTime publicationDate;
            if (DateTime.TryParse(elementsOfLine[3], out publicationDate))
            {
                publicationDate = publicationDate;
            }
            else
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} в столбце 4 записаны некорректные данные");
            }

            return publicationDate;
        }

        private static uint ReadBookId(int i, string[] elementsOfLine)
        {
            uint id;
            if (uint.TryParse(elementsOfLine[0], out id))
            {
                id = id;
            }
            else
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} в столбце 1 записаны некорректные данные");
            }

            return id;
        }

        private static void СheckBooksLineLength(int i, string[] elementsOfLine)
        {
            if (elementsOfLine.Length > 6)
            {
                throw new ArgumentException($"В файле Book.csv в строке {i + 1} столбцов больше чем 6");
            }
        }

        public static List<Reader> ReadReaders(string path)
        {
            List<Reader> readers = new List<Reader>();
            string[] allLinesReader = File.ReadAllLines(path);
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

        public static List<Statistic> ReadStatistics(string path, List<Book> books, List<Reader> readers)
        {

            List<Statistic> statistics = new List<Statistic>();
            string[] allLinesStatistics = File.ReadAllLines(path);

            for (int i = 0; i < allLinesStatistics.Length; i++)
            {
                string[] elementsOfLine = allLinesStatistics[i].Split(";");

                // парсим данные из одной строчки файла Statistics.csv
                uint id, readerId, bookId;
                DateTime takeDate, returnDate;
                ReadStatisticsLine(i, elementsOfLine, out id, out readerId, out bookId, out takeDate, out returnDate);

                Reader readerStatistics = ReadStatisticsReader(readers, i, readerId);

                Book bookStatistics = ReadStatisticsBook(books, i, bookId);

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

        private static Book ReadStatisticsBook(List<Book> books, int i, uint bookId)
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

        private static void ReadStatisticsLine(int i, string[] elementsOfLine, out uint id, out uint readerId, out uint bookId, out DateTime takeDate, out DateTime returnDate)
        {
            
            СheckStatisticsLineLength(i, elementsOfLine);

            id = StatisticsLineId(i, elementsOfLine);
            readerId = StatisticsLineReaderId(i, elementsOfLine);
            bookId = StatisticsLineBookId(i, elementsOfLine);
            takeDate = StatisticsLineTakeDate(i, elementsOfLine);
            returnDate = StatisticsLineReturnDate(i, elementsOfLine);

            СheckDate(i, takeDate, returnDate);
        }

        private static void СheckDate(int i, DateTime takeDate, DateTime returnDate)
        {
            if (takeDate > returnDate)
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} читатель вернул книгу до того как её взять");
            }
        }

        private static DateTime StatisticsLineReturnDate(int i, string[] elementsOfLine)
        {
            DateTime returnDate;
            if (DateTime.TryParse(elementsOfLine[4], out returnDate))
            {
                returnDate = returnDate;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 5 записаны некорректные данные");
            }

            return returnDate;
        }

        private static DateTime StatisticsLineTakeDate(int i, string[] elementsOfLine)
        {
            DateTime takeDate;
            if (DateTime.TryParse(elementsOfLine[3], out takeDate))
            {
                takeDate = takeDate;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 4 записаны некорректные данные");
            }

            return takeDate;
        }

        private static uint StatisticsLineBookId(int i, string[] elementsOfLine)
        {
            uint bookId;
            if (uint.TryParse(elementsOfLine[2], out bookId))
            {
                bookId = bookId;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 3 записаны некорректные данные");
            }

            return bookId;
        }

        private static uint StatisticsLineReaderId(int i, string[] elementsOfLine)
        {
            uint readerId;
            if (uint.TryParse(elementsOfLine[1], out readerId))
            {
                readerId = readerId;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 2 записаны некорректные данные");
            }

            return readerId;
        }

        private static uint StatisticsLineId(int i, string[] elementsOfLine)
        {
            uint id;
            if (uint.TryParse(elementsOfLine[0], out id))
            {
                id = id;
            }
            else
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} в столбце 1 записаны некорректные данные");
            }

            return id;
        }

        private static void СheckStatisticsLineLength(int i, string[] elementsOfLine)
        {
            if (elementsOfLine.Length > 5)
            {
                throw new ArgumentException($"В файле Statistics.csv в строке {i + 1} столбцов больше чем 5");
            }
        }

        private static Reader ReadStatisticsReader(List<Reader> readers, int i, uint readerId)
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
    }
}
