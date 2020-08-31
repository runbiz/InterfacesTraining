# Entities
The entities project is used only to store the entity-level classes of the business application. This is where your database will look to create it's entities from. Here we have our three main entity classes for this test application:
* **Account**
* **Developer**
* **Department**

These classes will not be referred to by the UI or displayed directly to the user. Instead, we will use **AutoMapper** and **DTO** classes to map our entity classes into *DTO* objects that display only the information we want our clients to see.

The entity classes also utilize data annotations to set up any validations/requirements/conditions of the entities *(ex. required fields, string length, foreign keys/relationships, etc.)*.

Our *Interfaces.Infrastructure* project will refer to these entity classes when setting up our *DbContext*.