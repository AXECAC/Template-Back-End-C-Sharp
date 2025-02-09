добавить enum на statusCode


operation result --- посмотреть варианты

builder.Services.AddSingleton --- посмотреть

Посмотреть

https://github.com/OfferingSolutions/Entity-Framework-Core-Generic-Repository
https://github.com/OfferingSolutions/Entity-Framework-Core-Generic-Repository/blob/master/OfferingSolutions.GenericEFCore/RepositoryBase/GenericRepositoryBase.cs
https://github.com/OfferingSolutions/Entity-Framework-Core-Generic-Repository/blob/master/OfferingSolutions.GenericEFCore.SampleApp/ExampleRepositories/PersonRepository.cs


Переделать UserRepository -> BaseRepository

```
return await Db.Users.FirstOrDefaultAsync(x => x.Email == email);
```
->
```
return await Db.Set<T>().FirstOrDefaultAsync(x => x.Email == email);
```

