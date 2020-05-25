# IMT3602 Professional Programming 

## Team Members
- Benjamin Bergseth (benjabe)
- Sigurd
- Stig
- Victor Sebastian Standal Clusen (vsclause)

## Programming Languages

### Lua
[MoonSharp](https://www.moonsharp.org/) is an open-source library that lets Lua scripts interface with C# using developer-defined bindings. This sort of interfacing means that you can easily open support for modding the game---the developer can control what parts of the game logic are exposed to modders through the C# bindings. 

The way Moonsharp loads files means you can also load variables for entities in the game from files during runtime, or even load scripts from lua files. This is excellent for games that do most of their logic on the server end, because you can issue balance updates or do testing without having to recompile the server---all that needs to change are the Lua files being read, and that can be done without any recompilation.

Lua can be very fast as a scripting language since it is so small and simple, though it does produce some overhead when compared to using compiled C# code since the code needs to be interpreted.

Another advange of using Lua was the ability to easily create a debug console which interpreted Lua. Since the interpretation had already been implemented, all that was needed was to add the actual input field, plus some other convenience features such as function suggestions and a command history.

Lua is free and is distributed under the MIT license, and can therefor be used for whatever the user wants to use it for.


### C#

C# is the scripting languaged used for Unity, so most guides, tutorials, questions, and help pages talk about C#, unless the tutorials specifically cover integrating another scripting language with Unity. Access to this knowledge base is very useful. 

C# uses a C-style syntax for its language rules, which makes it easier for the group to use since it is familiar with this type of language structure. The group also has a lot of direct experience with C#.

Garbage collection is used in C#. This is an advantage for ease of programming since the game developers don’t have to spend time and effort on manually managing memory, preventing certain types of memory leaks. One downside of using garbage collection is that one has less ability to manually optimize the code. However it is possible to do some manual memory management in C# in specific memory blocks that are marked as unsafe.

C# is a general purpose programming language, this means it can be used to develop several different types of systems, and is not very focused on a single area of use like a database language. This means one can use the same language to do networking, AI scripts, and game logic.

C# is a strongly typed language, which prevents certain type errors from occuring, since it will perform type checking on variables when actions are performed on them. In a strongly typed language variables are mapped to a data type so finding these kinds of type errors is doable at compile time, and some IDE’s will also detect them.

C# allows the use of namespaces, this allows for code to be organized in a structured manner, one can think of namespaces almost like a folder structure. C#’s namespaces is similar to java package system, or if one is familiar with the namespaces in C++. A namespace is kind of a region where one can use variable names without them coming into conflict with the same variable names in another namespace, also a good way to structure code. In our project we used namespaces for modular components such as the behaviour tree editor and utility AI system.

C# is a compiled language, which means that any time a change occurs there is a delay between saving the relevant file and being able to launch the project. In general compile times were small enough that this wasn't a hurdle for development.


## Process and Communication Systems
We communicated primarily over Discord throughout the whole project, and all meetings happened over Discord after the COVID-19 lockdown. We held meetings almost-daily at 10 am, which worked much like daily standing meetings where everyone said what they had worked on since last time, which problems they encountered, and what they would work on next. Every Monday at 10 am was dedicated to meeting with our supervisors where we took up problems we had in terms of process and project organisation, as well as style and content guidance for the report.

Trello was used for delegating tasks. This helped keep people accountable, but the board wasn't maintaned well enough, and the tasks on the board ended up not corresponding to the actual tasks needing to be developed. This resulted in the board being more or less abandoned after a while. We later used Trello for the thesis writing, with a review system included. We has less tasks and lists on the board, which made it a lot easier to maintain and navigate.

The group set up a Google Sheet where time worked by each member was tracked.

## Version Control system (Git)
Git was used for version control, which is the most used version control system for programming projects these days. The git repo was divided into two branches: a DevelopedBranch, and a master branch. We ended up not using the master branch much. We had a rule to not push code that would break the build or not compile, and this rule was followed, barring a few exceptions.

### Ticket Tracking
We used issues to some extent in our bitbucket repo, sometimes to different people. Mostly this was trying out something that was mentioned in the lectures, using issues for documentation and tying commits to specific issues. However, we found that this form of documentation was quite out-of-the-way compared to just writing relevant documentation in the file, or---our favored method---just asking the person developing the system what we need to know. Obviously this solution does not scale well for larger teams or longer term projects seeing as it is basically the communication equivalent of Code-and-Fix,  but for such a small team and such a short project time horizon, we found this a very low-overhead solution.

Here is an example showing how messaging works.
https://bitbucket.org/Arthurial/rts-minigame/issues/8/drone-messaging-system

One of the main causes of merge conflicts in Unity is when scenes differ from each other, to avoid these types of conflicts the group often had their own scenes they worked inn to prevent merge conflicts from occurring.

## Coding Standards

### Braces

For the brace styles it was decided to use the Allman style since it was the preferred choice for the project owner, and also the majority of the group members also preferred this bracket style. https://en.wikipedia.org/wiki/Indentation_style#Allman_style

When we made the project plan for the bachelor thesis the group also decided a lot of the programming guide lines. This list of naming is from the list we as a group decided upon.

### Variable Naming

- Private and protected member fields use underscore(_) as a prefix
- [PascalCase](https://techterms.com/definition/pascalcase) for Class and method/function names
- Variable names will use [camelCase](https://en.wikipedia.org/wiki/Camel_case)
- Don’t use [Hungarian notation](https://en.wikipedia.org/wiki/Hungarian_notation), except I prefix before interfaces
- Use variable names that are descriptive
- No need to use `this` keyword when your private and protected variable names uses underscores.
- Also we had a rule to not write type names into variable names. This was mostly followed, but for List’s we ended up some places writing the variable name in since it made knowing the functions you had access to very easy and fast.

### Comments
```csharp
/// <summary>
/// This is a method which does something useful.
/// </summary>
/// <param name="param">The parameter needed to do something useful.</param>
/// <returns>Something useful. Null if invalid.</returns>
private UsefulThing MethodName(int param)
{
    if (param < 0) return null;
    else return new UsefulThing();
}
```
The project used XML-style comments, which displays the comments, its parameters, return values and the like when the mouse is hovered over the method in question. It can be exported for documentation purposes.

## Libraries
In the project a few different libraries were used. One example of this is MoonSharp, which was used to get Lua to work with Unity. MoonSharp was installed using the Unity Asset Store for simple integration, and some scripts were modified to integrate with project-specific requirements.

Mirror was used for networking. This is an open-source library that people can use to developed networked video games in Unity.

mXparser was used to parse mathematical functions from Lua.

JsonSubTypes was used to serialise behaviour tree behaviours. This allowed subclasses of behaviours to easily be exported to JSON, without needing to make each subtype serialisable manually. This was downloaded directly from the GitHub repo.

## Professionalism
Things which were done with professionalism in mind were meetings, using Trello, and time tracking, all of which are covered elsewhere.

### Meetings with Project Owner
We had meetings with the project owner to try to nail down the specifics of what he wanted done for the project to the best of our abilities, though many of the requirements were somewhat vague and high-level, and much was up to the group's interpretation when it came time to implement features.

### Tone of Communication
Within the group nobody had any issues with using language which might be considered offensive. However, when engaging with people outside the group (project owner, supervisors) an effort was made to maintain a friendly and non-offensive tone.

### Ethics
We saw no ethical issues in the development of the project. Despite the thesis title, we did not develop real combat drones, and any repercussions of the drones' actions are confined to a fictional world, except the positive utility gained from player satisfaction when interacting with the drones. We did implement networking in the system, which means that some player data must likely be gathered for authentication purposes down the line. It is up to the project owner to handle the way this data is processed, and regulations should prevent him from doing anything that compromises their privacy.

### Intellectual Property
We made sure not to use any works we did not have the rights to. All libraries and assets used were found on the Internet licensed for free or installed through the Unity Asset Store.

## Tools

### The Internet
When we needed to learn code to implement algortihms or learn how to solve some specific problems releated to git, we often had to resort to using one of various search engines to find solutions on sites such as StackOverflow and YouTube. It goes without saying that this was massively helpful.

### Trello
We used two trello boards during this project, one Trello board that had a product backlog and another board was used when we were writing the thesis to assign topics to certain people, assign the person who would review the section. In the trello board for writing, if the user had enabled email notifcation then tehy would recive an email notification when they were added to a trello card.

## Code Review
We didn't use a code review for our system, except for LaTeX content and syntax later on in the project. The only time code was reviewed was when Simon reviewed a part of the behaviour tree system, and discovered some inefficiencies in the way the code was rewritten. This would merit a refactor, but priority had to be given to other aspects of the thesis, and, as such, the refactoring wasn't actually performed as a result of the code review.