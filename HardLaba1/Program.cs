namespace HardLaba1
{
    public class Program
    {   
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // Записываем данные из Reader.csv, Book.csv и Statistics.csv в соответствующие списки.
                List<Reader> readers = TableReader.ReadReaders("Data\\Reader.csv");
                List<Book> books = TableReader.ReadBooks("Data\\Book.csv");
                List<Statistic> statistics = TableReader.ReadStatistics("Data\\Statistics.csv", books, readers);

                // выводим таблицу на экран
                TableDisplay.TableDisplayStart(books,statistics,readers);
            }
            catch (ArgumentException ex)
            {
                Console.Clear();
                Console.Write("Ошибка:");
                Console.WriteLine(ex.Message);
            }
        }        
    }
}