using books.Domain;
using System.Numerics;

namespace books.Infrastructure
{
    public class DataService
    {
        public List<Book> Books { get; }

        public DataService()
        {
            Books = new()
            {
                new Book { Id = 1, Title = "Paragraf 22", Author = "Joseph Heller"},
                new Book {Id = 2, Title = "Gra w klasy", Author = "Julio Cortazar"},
                new Book {Id = 3, Title = "Pamiętnik żółtego psa", Author = "O'Henry"}
            };
        }
    }
}
