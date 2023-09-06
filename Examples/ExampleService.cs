using Examples.Db;
using Microsoft.EntityFrameworkCore;


namespace Examples
{
    public class ExampleService
    {
        private readonly ExampleContext _db;
        public ExampleService(ExampleContext db) 
        {
            _db = db;
        }


        public Task<ExampleEntity?> Get(Guid id)
        {
            return _db.Examples.FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<ExampleEntity?> GetNoTracking(Guid id)
        {
            return _db.Examples.AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Guid?> Add(ExampleEntity entity)
        {
            var res = _db.Examples.Add(entity);

            try
            {
                await _db.SaveChangesAsync();

                return res.Entity.Id;
            }
            catch
            {
                return null;
            }
        }

        public Task Update(ExampleEntity entity)
        {
            _db.Examples.Update(entity);

            return _db.SaveChangesAsync();
        }

        public Task UpdateGoesWrong(ExampleEntity updating)
        {
            try
            {
                throw new Exception("Unexpected exception");

                _db.Update(updating);

                return _db.SaveChangesAsync();
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

    }
}
