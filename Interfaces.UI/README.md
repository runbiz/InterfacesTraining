# UI
The **Interfaces.UI** project is the presentation layer that accesses our business logic through the **DAL** project and displays the relevant information to the client. In this case, our *UI* is a **.NET Core Razor Pages** web application. The **UI** project also contains the implementation of our *LoggingManager* service from our **Core** project, as well as utilizing the **AutoMapper** mapping Profiles we set up in **Core**.

## LoggerManager Implementation
There are a couple of steps to get *NLog* working in our application. First, we have created an ***nlog.config*** file that contains XML that NLog uses to determine where to save log files within our project, as well as some basic formatting options for the text files themselves. Currently, our Logging service is saving text log files in the **bin** folder of our local UI project.

To register the *LoggerManager* service in our UI application, we must add some configuration to our **Startup** file.