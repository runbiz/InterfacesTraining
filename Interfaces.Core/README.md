# Core
The Interfaces.Core project contains all logic/structures central to the business logic of the system. The Core project will be referenced by all other outer layers of the project. The Core project includes:
* DTO (Data Transfer Objects)
* Automapper Mapping Profile
* Logging Interface/Implementation
* Any Core-level models

### DTO (Data Transfer Objects)
Data Transfer Objects (DTOs) are used to transform entity-level model classes into client-level classes that only present the information/properties relevant to the client. DTOs are essentially the same thing as a View Model, but we get more granularity with DTOs by creating a specific DTO for each data transaction (Create, Read, Update).

For example, the **DeveloperDto** class is going to be the DTO used when loading a list of **Developer** objects, such as in a dropdownlist or a grid. The DTO model is very similar to the entity-level **Developer** class (located in *Interfaces.Entities* project), but the DTO is missing the data annotations, as they are not necessary for this DTO since we are just reading the data.

```csharp
public class DeveloperDto  
{  
    public Guid Id { get; set; }  
    public string Name { get; set; }  
    public DateTime DateOfBirth { get; set; }  
    public string Address { get; set; }
    public IEnumerable<AccountDto> Accounts { get; set; }
    public int DepartmentId { get; set; }
    public DepartmentDto Department { get; set; }
}
```
```csharp
public class Developer
{
    [Column("DeveloperId")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [StringLength(100, ErrorMessage = "Address cannot be loner then 100 characters")]
    public string Address { get; set; }

    public ICollection<Account> Accounts { get; set; }
    
    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}
```
In this example, the DTO and entity object are very similar, but there may be instances where additional properties need to be shown to the client, and you would create those properties on the **DeveloperDto** class.

Another example extends to the **DeveloperForCreationDto** class, which is used specifically for creating a new developer.
```csharp
public class DeveloperForCreationDto
{
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters.")]
    public string Name { get; set; }

    [Display(Name = "Date of Birth")]
    [Required(ErrorMessage = "Date of birth is required.")]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Address")]
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(100, ErrorMessage = "Address cannot be loner then 100 characters.")]
    public string Address { get; set; }

    // Relationships
    [Display(Name = "Department")]
    public int DepartmentId { get; set; }
}
```
As you can see above, this DTO does not include an *'Id'* property, as this is going to be a new record and will not have an ID. It also uses some data annotations to set up form validation for required fields and string lengths. Lastly, there is not an explicit relationship between **Developer** and **Department** in this DTO. There is only the *'DepartmentId'* property which will be used to set up the foreign key relationship on the entity class when adding the new developer.

These are just two examples of how DTOs can be used to help with organization. DTOs are great for keeping your entity class clean and simple, and also eliminates the need for a complicated View Model that contains dozens of properties that are intermittently used in different areas of the application. Each DTO contains only the exact information needed for the operation it is going to be used for, and in the case that there's not a DTO to match a new condition/feature, just make a new one!

DTOs are closely tied with the next topic, **AutoMapper**.

### AutoMapper
**AutoMapper** is a third-party package that is used to map an object from one type to another. In this case, we can use AutoMapper to map our DTO objects to entity objects, and vice versa. Using AutoMapper makes this process significantly easier, as we no longer have to manually need to map a view model (**DTO**) to an **entity** model. The implementation of AutoMapper happens in the *Interfaces.UI* project, but the configuration is being set here in the *Core* project. 

In order to use AutoMapper correctly, we must set up what is known as a Profile. This is a class that inherits the **AutoMapper.Profile** class, and in the constructor of this class, we use the CreateMap extension to specify all of our AutoMapper mappings.
```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Developer, DeveloperDto>();
        CreateMap<DeveloperDto, Developer>()
            .ForMember(m => m.Accounts, opt => opt.Ignore())
            .ForMember(m => m.Department, opt => opt.Ignore())
            .ForMember(m => m.DepartmentId, opt => opt.MapFrom(x => x.Department.Id));
        CreateMap<DeveloperForCreationDto, Developer>();
        CreateMap<DeveloperForUpdateDto, Developer>();

        CreateMap<Account, AccountDto>();

        CreateMap<Department, DepartmentDto>();
    }
}
```

As you can see above, we are creating several maps to use in our application. Focusing on the Developer maps, we have one map that maps our **Developer** entity class into our **DeveloperDto** DTO class. We are also doing the opposite and mapping from our **DeveloperDto** DTO class to the **Developer** entity class, and this mapping includes some custom rules for mapping certain members of the **DeveloperDto** class to the **Developer** entity class. 

With AutoMapper, it expects property names to match between the models in order to perform the mapping correctly, so the custom rules can come in handy in the case that there are different property names between the models, or if property types are not the same. 

There are lots more configuration options available to browse in **AutoMapper's** official documentation, but this is just a simple example. To see how to implement an AutoMapper mapping that we've created a profile for, refer to the backend page model of */Pages/Developer/jQuery/Index* in the *Interfaces.UI* project.

### Logging Interface/Implementation
In this application, we are using a third-party logging package called NLog to keep text logs of information and errors as request are made in the web application. To create an easy to use logging service, we've implemented an *ILoggerManager* interface that includes methods for logging Information, Errors, Warnings, and Debug messages. Then, the *LoggerManager* class inherits the *ILoggerManager* contract and contains actions to log the messages passed to each action.

The injection/implementation of the *LoggerManager* happens in the *Interfaces.UI* project.