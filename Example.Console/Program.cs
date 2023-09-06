using Examples;
using Examples.Db;

await Example1();
await Example1AsNoTracking();
await Example2();
await Example2AsNoTracking();
await Example3();
await Example3AsNoTracking();

async Task Example1()
{
    var db = new ExampleContext();

    var service = new ExampleService(db);

    var entity = new ExampleEntity()
    {
        Counter = 0,
        Title = "Test"
    };

    var id = await service.Add(entity);
    var getResult = await service.Get(id.Value);

    getResult.Counter = 1000;
    await service.UpdateGoesWrong(getResult); //this call would never reach SaveChangesAsync()

    await service.Add(new ExampleEntity { Counter = 1, Title = "Test1" }); //this call saves first Example entity changes, cause it's tracked by EF

    var getResult2 = await service.Get(id.Value);
    Console.WriteLine(getResult2.Counter); //get Counter = 1000

    db.ChangeTracker.Clear();
    var getResult3 = await service.Get(id.Value);
    Console.WriteLine(getResult3.Counter); //get Counter = 1000, even if Tracker is empty
}

async Task Example1AsNoTracking()
{
    var db = new ExampleContext();

    var service = new ExampleService(db);

    var entity = new ExampleEntity()
    {
        Counter = 0,
        Title = "Test"
    };

    var id = await service.Add(entity);
    var getResult = await service.GetNoTracking(id.Value);

    getResult.Counter = 1000;
    await service.UpdateGoesWrong(getResult); //this call would never reach SaveChangesAsync()

    await service.Add(new ExampleEntity { Counter = 1, Title = "Test1" }); //this call saves first Example entity changes, cause it's tracked by EF

    var getResult2 = await service.GetNoTracking(id.Value);
    Console.WriteLine(getResult2.Counter);
    Console.WriteLine(db.ChangeTracker.DebugView.LongView);
}

async Task Example2()
{
    var db = new ExampleContext();

    var service = new ExampleService(db);

    var entity = new ExampleEntity()
    {
        Counter = 0,
        Title = "Test"
    };

    var id = await service.Add(entity);
    var getResult = await service.Get(id.Value);

    getResult.Title = null;

    try
    {
        await service.Update(getResult); //this call would fail on SaveChangesAsync()
    }
    catch { }

    var getResult2 = await service.Get(id.Value);
    Console.WriteLine(getResult2.Title); //get Title = null, cause this change is tracked by EF 
    Console.WriteLine(db.ChangeTracker.DebugView.LongView);
    db.Dispose();


    db = new ExampleContext();

    service = new ExampleService(db);

    var getResult3 = await service.Get(id.Value);

    Console.WriteLine(getResult3.Title); //get the actual value of Title - "Test"
}

async Task Example2AsNoTracking()
{
    var db = new ExampleContext();

    var service = new ExampleService(db);

    var entity = new ExampleEntity()
    {
        Counter = 0,
        Title = "Test"
    };

    var id = await service.Add(entity);
    var getResult = await service.Get(id.Value);

    getResult.Title = null;

    try
    {
        await service.Update(getResult); //this call would fail on SaveChangesAsync()
    }
    catch { }

    var getResult2 = await service.GetNoTracking(id.Value);
    Console.WriteLine(getResult2.Title); //get Title = "Test"

    Console.WriteLine(db.ChangeTracker.DebugView.LongView);
}


async Task Example3()
{
    var db = new ExampleContext();
    var service = new ExampleService(db);

    var id = Guid.NewGuid();

    var entity = new ExampleEntity()
    {
        Id = id,
        Counter = 0,
        Title = "Test"
    };

    db.Add(entity);

    var res = await service.Get(id); //get the entity, even if it is not saved to the database

    Console.WriteLine(db.ChangeTracker.DebugView.LongView);
}

async Task Example3AsNoTracking()
{
    var db = new ExampleContext();
    var service = new ExampleService(db);

    var id = Guid.NewGuid();

    var entity = new ExampleEntity()
    {
        Id = id,
        Counter = 0,
        Title = "Test"
    };

    db.Examples.Add(entity);

    var res = await service.GetNoTracking(id); //get null, cause entity is not saved to the database

    Console.WriteLine(db.ChangeTracker.DebugView.LongView);
}