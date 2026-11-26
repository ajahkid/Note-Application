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

## Milestone 03: Layered Multi-user Console Application

This version adds user registration, login, and logout. Each note belongs to
one user, and the menu only shows and changes the current user's notes.
Note IDs stay with the notes instead of being their positions in a list.

The code is organized into three layers:

- **Presentation:** `ConsoleMenu` displays menus and reads input.
- **Service:** `NoteService` checks input, manages the current user, and
  applies note ownership rules.
- **Data access:** `JsonRepository` loads and saves users and notes as JSON.

The models describe users and notes. Passwords are stored as salted PBKDF2
hashes rather than plain text. This is a learning project: passwords are
visible while typing, note contents are not encrypted, and anyone with access
to the JSON file can read or change it. Do not use real passwords or sensitive
notes. The app is designed for one running instance at a time.

### How to run

Install the .NET 10 SDK. Open a terminal in the project root and run:

```powershell
cd Milestone03_LayeredConsole
dotnet run
```

Register with a username and a password of at least eight characters, then log
in. Note IDs are shown when listing notes and are used to edit or delete them.
The app saves to `milestone03-data.json` in the terminal's current folder.
Its data is separate from the earlier milestones and is not automatically
transferred. The file is ignored by Git when running from the project root or
milestone folder.

## Milestone 04: Database Application

This version keeps the multi-user console application but replaces JSON
storage with a SQLite relational database using Entity Framework Core (EF
Core). The `Users` and `Notes` tables each have an `Id` primary key.
`Notes.OwnerId` is a foreign key to `Users.Id`: one user can own many notes,
and each note has one owner. Usernames have a unique normalized value so
registration and login are case-insensitive.

The repository uses parameterized SQL for user and owner-specific note
queries. EF Core saves new and changed records. Additional example SQL
queries are in `Milestone04_Database/example-queries.sql`. SQLite's native
package is pinned separately to avoid a known vulnerability in the older
transitive version.

### How to run

Install the .NET 10 SDK. Open a terminal in the project root and run:

```powershell
cd Milestone04_Database
dotnet run
```

The first run restores the required packages and creates
`milestone04-data.db` in the terminal's current folder. Register and log in
to manage notes. The database is separate from previous milestone files; data
is not automatically imported. The database is ignored by Git when running
from the project root or milestone folder. Back up the database if needed.

This educational app is intended for one running instance at a time.
Passwords are hashed, but are visible when typed; note contents and the
database are not encrypted. Do not use real passwords or sensitive notes.
EF Core uses `EnsureCreated` for this milestone; it creates a new database
but does not update an existing schema when the models change.

## Planned milestones

- **Milestone 05:** Add an ASP.NET Core Web API for managing notes.

These milestones will be added as they are built and tested.
