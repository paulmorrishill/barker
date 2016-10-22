# Barker

Have you ever wanted to share your thoughts online and feel like people are reading and paying attention to you? Then this is the project for you.

# Development environment set up

You will need an IDE capable of doing work on .Net projects. You can use [visual studio community addition](https://www.visualstudio.com/vs/community/). Or if you feel confident enough you can also use [Jetbrains Project Rider EAP 1.0](https://www.jetbrains.com/rider/) which is pretty nice.

For UI work you'll need some sort of decent javascript IDE like jetbrains Webstorm/IntelliJ. Or you can use a plain text editor if you are bit mental.

# Basic structure

This project is separated into 3 layers
- Persistence layer (currently uses a flat file storage implementation)
- Business logic layer (C#)
- Delivery mechanism (C# using nancy to expose a REST API + UI of Angular+twitter bootstrap)

Each layer is tested independently. The data access and logic are tested using unit tests. 
The UI+API is tested using Selenium, this allows for extremely fast test suites for both code and UI, which facillitates TDD on both fronts. 

# Installation

- Clone the git repo
- Open a command prompt and cd into UserInterface
- Run ```npm install``` and ```bower install```
- Run ```npm install -g grunt```

# Running tests

Before doing any dev work you should run all the tests to make sure you environment is set up correctly. 
All dev work should be be tested, preferably using TDD.

## .Net unit tests and UI tests

- Run ```grunt serve``` from the UserInterface directory. This will serve the UI for the UI tests.
- Open the main solution file your .net IDE of choice.
- Build the solution
- Run all tests

## UI Unit tests

- Run ```grunt test``` from the command line inside the UserInterface directory 

# Running in "production"

- Open your .Net IDE
- Set the ConsoleServer project as your startup project
- Run the console server
- Run ```grunt serve``` from the UserInterface directory.
- Navigate to http://localhost:9000

