# EntityFrameworkIncludeBuilder
`EntityFrameworkIncludeBuilder` is an Entity Framework Core extensions which provide ability to build dynamically `Include/ThenInclude` chain.

## Usage
### 1. Define repository method.
We add method which provide ability to get all items of `TEntity` type and specify all necessary navigation properties. The user can specify only those navigation properties that he will use.


```csharp
// ...
using EntityFrameworkIncludeBuilder;
// ...
{
  // ...
  public IEnumerable<TEntity> GetAll<TEntity>(
          Expression<Func<IEnumerable<TEntity>, 
          ILoadCollection<TEntity, object>>> includes = null)
  {
      var query = Table.AsQueryable(); //  DbSet<TEntity>
      return includes == null
          ? query
          : query.Include(includes);
  }
  // ...
}
```
### 2. Sample
```csharp
//...
var items = _repository
      .GetAll(collection => collection.Load(x => x.Property1)
                                      .Load(x => x.Collection1)
                                        .ThenLoad(x => x.Property2)
                                          .ThenLoad(x => x.Property3));
//...
```

## Contribute

One of the easiest ways to contribute is to report issues and participate in discussions on issues. You can also contribute by submitting pull requests with code changes and supporting tests.

## License

[MIT](https://github.com/dorohin/EntityFrameworkIncludeBuilder/blob/master/LICENSE)
