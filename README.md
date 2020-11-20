# MongoSimplified

Cette librairie permet de faciliter la connection à une base de donnée MongoDb et d'effectuer les principales opération de CRUD.

## Initialisation

### Création de la connection
Créer une class qui hérite de ```BaseContext```.
Spécifiez les propriétés ``ConnectionString`` et ``DatabaseName``.
```
public class ContextTest : BaseContext
{
    public sealed override string ConnectionString { get; } = "mongodb://127.0.0.1:27017";
    public sealed override string DatabaseName { get; } = "DataBaseTest";

    public ContextTest()
    {
        Client = new MongoClient(ConnectionString);
        MongoDatabase = Client.GetDatabase(DatabaseName);
    }
}
 ```
 
### Création des class de models
Les entités qui doivent être enregistrés dans la base de données doivent hériter de ``Entity``.
Chaque entité sera enregistré dans une collection du même nom que l'entité. 
```
public class Parent : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Adress { get; set; }
    public string Cp { get; set; }
    public string City { get; set; }

    public string FamilyId { get; set; }
}
```

## Utilisation
Dans une méthode, si vous souhaitez utiliser la connection à la base de données, vous devez l'instancier avec un ``using`` car ``BaseContext`` hérite de ``IDisposable``
```
public void InsertData()
{
  using var context = new ContextTest(); 
  
  var parent = new Parent
  {
    FirstName = "Deligans",
    LastName = "Fabien",
  }
  
  context.Insert(parent); 
}
```

