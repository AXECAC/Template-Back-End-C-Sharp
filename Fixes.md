- МБ потом: почитать middleware для обработки ошибок (вместо try|catch) и для авторизации

- baseresponse -> статический фабричный метод 

  BaseResponse<T>.Ok();

  Написать BaseResponse.Ok() класс без поля data


- Переписать поиск по Id и по Email на...
  ```cs
    public async Task<T>? FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
    {
        return await Db.Set<T>().FirstOrDefaultAsync(expression);

    }
  ```
  В BaseRepository

- В BaseRepository стоит добавить метод IQuariable<T> Where(Expresion)
- IQuariable Select(Expresion)

```cs
using System.Collections.Generic;
// Достать Id всех юзеров с именем Alex
var .. = await UserRepository
  .Where(x => x.FirstName == "Alex")
  .Select(x => x.Id)
  .ToListAsync();
```

```cs
// Достать всех юзеров с Id > 10 группируя по имени
var .. = await UserRepository
  .Where(x => Id > 10)
  .GroupBy(x => x.FirstName)
  .ToListAsync();
```

