# DAL
The **DAL *(Data Access Layer)*** project is the layer that facilitates communication between the *Interfaces.Infrastructure* and *Interfaces.UI*. Rather than injecting the *DbContext* directly into the web application page models, we use a repository pattern that accesses the database and performs CRUD operations and is wrapped in a single wrapper that we can inject into our web application and use for data access.

The **DAL** is made up of two sections:
* Contracts
  * The *Contracts* folder contains all of the interfaces (contracts) that are used by the repositories. These contracts are essentially 'rules' that state exactly what methods the repositories that inherit the contracts must implement. There are also interfaces for the ***RepositoryBase***, which is a generic repository pattern for CRUD operations that all other repositories interfaces implement (more on this below). Lastly, there is an interface for the ***RepositoryWrapper***, which combines our individual repositories under one DB-connected wrapper that we can inject into our web application.
* Repositories
  * The *Repositories* folder contains all of the repository classes that inherit from the *Contracts*. By inheriting from both the ***RepositoryBase*** and the entities' relative repository contract, the repository class gains access to all generic CRUD operations from the ***RepositoryBase***, as well as any extended actions set up by the entities' repository contract.

## Generic Repository Pattern
The **DAL** project implements a design pattern called the *Generic Repository Pattern*. What this means is that there is a generic repository interface/class that uses a generic Type<T> to build the contract.
```csharp
public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
```
As you can see above, the contract includes methods for CRUD operations using the generic type <T>, which we will pass our desired entity to in the implementation of the repository.

The generic repository class **RepositoryBase** implements the **IRepositoryBase<T>** contract and has our *DbContext* injected to be used with our CRUD operations. The methods here are very simple:
* **FindAll** - Returns an *IQueryable* list of the entity.
* **FindByCondition** - Returns an *IQueryable* list of entities based on the entities that meet the criteria expression passed to the method.
* **Create** - Adds the passed entity to the context.
* **Update** - Updates the passed entity in the context.
* **Delete** - Deletes the passed entity from the context.

With this base class established, we can now inherit this abstract class in any of our entity-specific repositories *(ex. DeveloperRepository)* and gain access to all of these CRUD methods so we don't have to retype any CRUD operations. This eliminates duplicate boilerplate code as we can let the **RepositoryBase** class perform any CRUD operations and leave our entity-specific repositories to focus on running actions specific to the entity.

## Repositories
With our base generic repository established, we can now focus on our entity repositories. We will look at *DeveloperRepository* to get a sense of how it works.
#### IDeveloperRepository.cs
```csharp
public interface IDeveloperRepository : IRepositoryBase<Developer>
    {
        Task<IEnumerable<Developer>> GetAllDevelopersAsync();
        Task<IEnumerable<Developer>> GetAllDevelopersWithDetailsAsync();
        Task<IEnumerable<Developer>> GetDevelopersByDepartmentAsync(int departmentId);
        Task<Developer> GetDeveloperByIdAsync(Guid developerId);
        Task<Developer> GetDeveloperWithDetailsAsync(Guid developerId);
    }
```
As you can see, our *IDeveloperRepository* contract sets up a series of entity-specific asynchronus actions. The implementation of these rules are below.
### DeveloperRepository.cs
```csharp
public class DeveloperRepository : RepositoryBase<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(InterfacesContext interfacesContext) : base(interfacesContext)
        {
        }

        public async Task<IEnumerable<Developer>> GetAllDevelopersAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Developer>> GetAllDevelopersWithDetailsAsync()
        {
            return await FindAll()
                .Include(d => d.Accounts)
                .Include(d => d.Department)
                .OrderBy(ow => ow.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Developer>> GetDevelopersByDepartmentAsync(int departmentId)
        {
            return await FindByCondition(d => d.DepartmentId.Equals(departmentId))
                .Include(d => d.Department)
                .ToListAsync();
        }        

        public async Task<Developer> GetDeveloperByIdAsync(Guid developerId)
        {
            return await FindByCondition(d => d.Id.Equals(developerId))
                .FirstOrDefaultAsync();
        }

        public async Task<Developer> GetDeveloperWithDetailsAsync(Guid developerId)
        {
            return await FindByCondition(d => d.Id.Equals(developerId))
                .Include(d => d.Accounts)
                .Include(d => d.Department)
                .FirstOrDefaultAsync();
        }
    }
```
As you can see, our *DeveloperRepository* class implements both our *IDeveloperRepository* contract as well as our abstract *RepositoryBase* class, giving our repository access to all of the generic methods provided by the base class. A good example of this is in the **GetDevelopersByDepartmentAsync** Task. This is an async Task that returns a list of **Developers** where their *'DepartmentId'* matches the *departmentId* parameter. In this method, we are accessing the **FindByCondition** method of the *RepositoryBase*, which accepts an expression. We are passing the expression as a *LINQ* query, and after it returns all entities that match the condition, we use **EF** to include our foreign **Department** entities, then return a List. This code is very clean and readable, and this extends to the other Tasks in our repository. The idea here is that as new actions are needed, we add them to our repository/contract and will access them through our *UI*, keeping all data access logic here, rather than in our front-end application.

You may have noticed that our **DeveloperRepository** does not include any methods for Create, Update, or Delete. Since we inherited the *RepositoryBase* class, we have access to those methods not only here in our repository class, but also in our front-end when we make a call to the **DeveloperRepository**. Since we don't require any additional functionality when Creating, Updating, or Deleting a **Developer** entity, there is no reason to implement methods for those in our repository. However, if we did need to extend the behavior of the generic Create, Update, or Delete methods, that is pretty easy to implement. All we would need to do is add a rule in our **IDeveloperRepository** contract for Creating a Developer, and implement it in our **DeveloperRepository**. I've included a simple example below of what it would look like if we needed to extend the behavior of our Create action for the **Developer** entity.

### IDeveloperRepository.cs
```csharp
public interface IDeveloperRepository : IRepositoryBase<Developer>
    {
        Task<IEnumerable<Developer>> GetAllDevelopersAsync();
        Task<IEnumerable<Developer>> GetAllDevelopersWithDetailsAsync();
        Task<IEnumerable<Developer>> GetDevelopersByDepartmentAsync(int departmentId);
        Task<Developer> GetDeveloperByIdAsync(Guid developerId);
        Task<Developer> GetDeveloperWithDetailsAsync(Guid developerId);
        
        // Here is my new action for creating a Developer. The Create method doesn't need to return anything, so it returns void.
        void CreateDeveloper(Developer developer);
        //****************************************
    }
```
Now that the **CreateDeveloper** action has been added to the contract, let's implement it in the **DeveloperRepository**.
```csharp
public class DeveloperRepository : RepositoryBase<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(InterfacesContext interfacesContext) : base(interfacesContext)
        {
        }

        public async Task<IEnumerable<Developer>> GetAllDevelopersAsync()
        {
            return await FindAll()
                .OrderBy(ow => ow.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Developer>> GetAllDevelopersWithDetailsAsync()
        {
            return await FindAll()
                .Include(d => d.Accounts)
                .Include(d => d.Department)
                .OrderBy(ow => ow.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Developer>> GetDevelopersByDepartmentAsync(int departmentId)
        {
            return await FindByCondition(d => d.DepartmentId.Equals(departmentId))
                .Include(d => d.Department)
                .ToListAsync();
        }        

        public async Task<Developer> GetDeveloperByIdAsync(Guid developerId)
        {
            return await FindByCondition(d => d.Id.Equals(developerId))
                .FirstOrDefaultAsync();
        }

        public async Task<Developer> GetDeveloperWithDetailsAsync(Guid developerId)
        {
            return await FindByCondition(d => d.Id.Equals(developerId))
                .Include(d => d.Accounts)
                .Include(d => d.Department)
                .FirstOrDefaultAsync();
        }

        // Here's my new method for creating the Developer. This is just a simple example, and we are just overriding the 'Name' property to "Test Developer".
        public void CreateDeveloper(Developer developer)
        {
            developer.Name = "New Developer";
            Create(developer);
        }
    }
```
In the example above, we're just overriding the *'Name'* property and setting it to *"New Developer"*. Notice that we're still calling the **Create** method from the **RepositoryBase**. We are still free to use the generic method here, and we are just updating the model before we call it.

Now that our repository has been updated, when we need to create a developer in our UI application, we can either call **Create**, which will create our **Developer** entity as-is, or instead call our new method **CreateDeveloper**, which will override the *'Name'* property before creating the **Developer**.

This has been a simple overview of how the repository pattern can be used to make CRUD operations easier and prevent repeating yourself, and also shows how beneficial repositories can be to make structured methods that are easy to read and use in our UI applications.

## RepositoryWrapper
The last piece of the **DAL** project is the *RepositoryWrapper* interface/class. This is simply a wrapper interface that includes instances of all of your entity repositories, as well as exposing an async Task called *SaveAsync*. The implementation of the interface uses dependency injection to inject each of the repositories into the class, as well as the *DbContext*. Then, it sets each repository to a new instance and provides each of them the DbContext to use. Lastly, the *SaveAsync* Task simply saves the context. The advantage of this is that in an instance where we need to perform multiple repository actions in our UI (ex. Adding a Developer, deleting another Developer, updating a Department), we can execute them all, then call *SaveAsync* once at the end and it updates the DbContext with all our new changes.

Now that our wrapper is set up, we can register it as a service in our UI application and use dependency injection to inject it into the backend of any page we need it on. The advantage of using a **RepositoryWrapper** is it simplifies our injection process. Instead of separately injecting **IDeveloperRepository, IAccountRepository,** and **IDepartmentRepository** into every page we need them on, we only need to inject **RepositoryWrapper**, and we have access to all of our repositories *(ex. _repositoryWrapper.Developer.GetAllDevelopersAsync)*. 
