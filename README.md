# InterfacesTraining Solution
This solution contatins the basic framework of a *.NET Core Razor Pages Web Application* using a backend that focuses on a simple version of SOLID design principles. It consists of 5 projects:
* **Interfaces.Entities** - The *Entities* project contains the entity-level business class models that will be used throughout the system, and will also be the objects that our database uses.
* **Interfaces.Infrastructure** - The *Infrastructure* project contains the instantiation of our DbContext as well as the objects tied to the database.
* **Interfaces.Core** - The *Core* project is the backbone of the application. It contains all DTO classes, our Logging service, our AutoMapper mapping profiles, and any common models used throughout the system.
* **Interfaces.DAL** - The *DAL* project is our data access layer that facilitates communication between our *Infrastructure* DbContext and our front-facing *UI* project using the Generic Repository pattern for CRUD operations.
* **Interfaces.UI** - The *UI* project is our .NET Core Razor Pages Web Application that uses the *DAL* and *Core* projects to load data from our *DAL* and display it to the client appropriately, as well as handling validation, errors, and logging.

There are README.md files in each project that go into detail regarding exactly what each element of the different projects are doing in regards to the application as a whole.
