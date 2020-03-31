# EntityFrameworkIncludeBuilder

[![NuGet](https://img.shields.io/badge/nuget-v2.0.0-blue)](https://www.nuget.org/packages/EntityFrameworkIncludeBuilder/2.0.0)
[![NuGet](https://img.shields.io/github/license/dorohin/EntityFrameworkIncludeBuilder)](https://github.com/dorohin/EntityFrameworkIncludeBuilder/blob/master/LICENSE)

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

## Donation
If this project help you reduce time to develop, you can give me a cup of coffee :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=2XKUL52KAA8Q8&source=url)
