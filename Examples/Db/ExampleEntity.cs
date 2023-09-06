using System.ComponentModel.DataAnnotations;

namespace Examples.Db
{
    public class ExampleEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public int Counter { get; set; }
    }
}