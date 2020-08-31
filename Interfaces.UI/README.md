# UI
The **Interfaces.UI** project is the presentation layer that accesses our business logic through the **DAL** project and displays the relevant information to the client. The **UI** project also contains the implementation of our *LoggingManager* service from our **Core** project, as well as utilizing the **AutoMapper** mapping Profiles we set up in **Core**. In this case, our *UI* is a **.NET Core Razor Pages** web application.

## Startup Configuration
To get our application working using all of our DAL, Logging, and any other services/extensions/configuration, we must add some code to our *Startup* file.
### Startup.cs Configure Services
```csharp
public void ConfigureServices(IServiceCollection services)
{
    /********************** Configured in Extensions\ServiceExtensions.cs **********************/

    // Configures the DbContext and ties it to the connection string in appsettings.json
    services.ConfigureDbContext(Configuration);

    // Adds any custom-built services to the application (ex. ILoggerManager, IRepositoryBase)
    services.ConfigureCustomServices(Configuration);

    // Adds AutoMapper to the application and binds our MappingProfile class to it 
    services.ConfigureAutoMapper(Configuration);

    // Adds Razor Pages, Controllers, and any other web-based services to the application
    services.ConfigureWebServices(Configuration);
    
    /******************************************************************************************/

    services.AddKendo();
}
```
In the image above, we are configuring all services/extensions that we need in our application. All of these services are being implemented in another file called **'ServiceExtensions.cs'**, located at *~/Extensions*. These extension methods include setting up our DbContext using our connection string in appsettings.json, adding instances of both our *ILoggerManager* and *IRepositoryBase* services, adding AutoMapper as a service to our applicatio and directing it to use our *MappingProfile* from **Interfaces.Core**, and adding services for Razor Pages, Controllers, and any other web services.

### LoggerManager Implementation
There are a couple of additional steps to get *NLog* working in our application. First, we have created an ***nlog.config*** file that contains XML that NLog uses to determine where to save log files within our project, as well as some basic formatting options for the text files themselves. Currently, our Logging service is saving text log files in the **bin** folder of our local *UI* project.

To register the *LoggerManager* service in our UI application, we must add some configuration to our **Startup** file.

#### Startup.cs
```csharp
public Startup(IConfiguration configuration)
{
    // This is used by the NLog package to check it's config file to determine the location to save the log file
    LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
    Configuration = configuration;
}
```
In the **Startup** constructor, we must load the configuration from our *nlog.config* file using the directory we are storing the log file in.

With this done, and our application registering an instance of our **ILoggerManager** service, we are all set to use logging through *NLog* in our application.

## Razor Page Example
To get a sense of how the implementation of our business logic and DAL work in a UI application, take a look at the page model of the Developer *Index* page in the UI project ***(/Pages/Developer/jQuery/Index.cshtml)***. It's broken down into three regions:
* **DI/Constructor** - This is where any services registered in our *Startup* file are injected into the constructor of our page model for use throughout the backend of the page. You can see here we're injecting our **IRepositoryWrapper** to access our *DAL*, our **ILoggerManager** that will allow us to use logging in our methods, and **IMapper**, which is AutoMapper's interface to map entity objects to DTO's to display to the client.
* **Page Model Properties** - This is where any properties are created that are going to be used on the front end of the page. Here we have a *DeveloperForCreationDto* object being instantiated, and it is decorated with the **'[BindProperty]'** data annotation. This allows us to use the object in the front end of our page and allows us to bind it to a form. We also have a string for *StatusMessage*, which is decorated with the ***'[TempData]'*** annotation. This binds the property to the page in a temporary dictionary that is only available on the next request, so we can use this for properties that we only want to show/use on the very next instance of the page, which works well for a status message.
* **Page Methods** - This region is where all the methods/operations of the page take place. This includes CRUD operations for a *Developer*, as well as getting a list of *Departments* for use in a dropdown on the insert/edit forms. In setting up our actions, we are following what is known as *'conventional routing'*. This means we are setting up our routes to operate within conventions inherent to .NET Core, rather than manually creating a route for each method. More specifically, we are using conventional routing with a focus on HTTP verbs (GET, POST, PUT, DELETE) to specify each route. There are a couple of things to consider when looking at these routes:
   * Razor Pages routes use a prefix convention on every method, and you can see examples of this in every method if you look at the beginning of the method name. *'OnGet', 'OnPost', 'OnPut'*, and *'OnDelete'* are all prefixes being used on our different methods, and what this does is indicate at runtime that these methods will run as a *GET, POST, PUT*, or *DELETE* request respectively.
   * If we have multiple of the same type of request, or want to use a more specific name on any of our requests, we can add a suffix to the method name that is referred to as the *'handler'* of the action. For example, we have 2 GET requests on the page, one each for Developers and Departments. To define these separately, we have included a suffix for *Developers* and *Departments* on their respective actions, and this allows us to call either of these using AJAX by passing the 'handler' name in the query string. 
   * Another note: Since our DAL is set up for async operations, we are making all calls to our DAL asynchronously, so all of our action methods are *'async Task<IActionResult>'*, rather than just an *'IActionResult'*. We are also adding another suffix to the end of our method names, *'Async'*, which is just a naming convention to help identity when a method is executing async code. The *'Async'* portion of the name does not need to be included in the route when using AJAX.

### GET Developers
To understand how to put everything we've done together, let's look at the *'OnGetDevelopersAsync'* action. This method returns a list of *DeveloperDto* objects as JSON that will be used by **KendoUI** on the front end of the page to populate the grid.
```csharp
public async Task<IActionResult> OnGetDevelopersAsync()
{
    try
    {
        var devs = await _repository.Developer.GetAllDevelopersWithDetailsAsync();
        _logger.LogInfo($"Returned all developers from database.");

        var devsResult = _mapper.Map<IEnumerable<DeveloperDto>>(devs);
        return new JsonResult(devsResult);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Something went wrong inside GetDevelopers action: {ex.Message}");
        StatusMessage = "Error: Could not find developers.";
        return StatusCode(500, "Internal server error");
    }
}
```
As you can see above, in our *try* block, we are making an awaited call to our **IRepositoryWrapper**, targeting the *Developer* repository, and using the method *GetAllDevelopersWithDetailsAsync()*. This will hit our *DeveloperRepository*, which is set up to grab all developers, add them to a list, and include any navigation properties *(Departments)*. If it succeeds, we will use our **ILoggerManager** service to log information stating that all developers are being returned. Lastly, we are taking our **IEnumerable<Developer>** list returned from the DAL and using *AutoMapper* to map the list to an **IEnumerable<DeveloperDto>** so that we are only passing a list of the relevant properties to the client in JSON format. In case an exception is thrown, we are using our **ILoggerManager** to log an error message and descriptor to our log file, then updating our *StatusMessage* Temp Data property and returning a Server 500 error.

The *POST*, *PUT **(Update)***, and *DELETE* actions function in the same manner, with some added functionality such as checking that the ModelState is valid, or grabbing the existing Developer by *ID* to perform an update/delete.

Using this approach, you can see that our methods are very clean and simple, with all data access happening outside of the page model. This makes it easy to manage the page model code by giving it only the reposibilities of checking validation, accessing a service, formatting the data returned/sent, and logging/returning success or error messages and results.