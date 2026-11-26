# Note Application

The Note Application is a C# project for saving and finding simple text
notes. It is being built in stages to show how the application grows as new
backend and database concepts are introduced.

## Milestone 01: Basic Console Application

This version runs in the console. It can add, list, find, edit, and delete
notes. Notes are stored in a list while the program is running and saved to a
JSON file so they are available again after the program closes.

The code uses variables, conditions, loops, methods, and a collection.
It checks for empty notes, empty searches, invalid menu choices, and invalid
note numbers.

### How to run

Install the .NET 10 SDK. Open a terminal in the project root and run:

```powershell
cd Milestone01_BasicConsole
dotnet run
```

Choose an option from the menu. To find a note, enter text that appears in it;
the search is not case-sensitive. To edit or delete a note, enter its number
from the list. Note numbers can change after a note is deleted.

The app creates `notes.json` in the terminal's current folder. Running the
commands above stores it in `Milestone01_BasicConsole`. The file is ignored by
Git to avoid accidentally committing personal notes. Keep a backup if the
notes are important.

## Milestone 02: OOP and Persistence

This version keeps the same console menu and JSON storage, but uses a `Note`
class instead of storing each note as a single string. A note has a title and
content. The constructor creates a note, and the `Update` method changes its
properties. The properties have private setters, so they cannot be changed
directly from outside the class.

The menu can add, list, find, edit, and delete notes. Search checks both the
title and content. Both fields are required.

### How to run

Install the .NET 10 SDK. Open a terminal in the project root and run:

```powershell
cd Milestone02_OOP
dotnet run
```

This version saves to `milestone02-notes.json` in the terminal's current
folder. It uses a separate file because its note format is different from
Milestone 01; notes are not automatically transferred between the versions.
The file is ignored by Git when running from the project root or milestone
folder.

## Planned milestones

- **Milestone 03:** Separate the console, services, and data access; add users
  and owner-specific notes.
- **Milestone 04:** Replace JSON storage with a relational database and show
  the relationship between users and notes.
- **Milestone 05:** Add an ASP.NET Core Web API for managing notes.

These milestones will be added as they are built and tested.
