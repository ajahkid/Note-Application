-- View users without displaying password data.
SELECT Id, Username FROM Users;

-- View one user's notes. Replace 1 with the user's ID.
SELECT Id, Title, Content FROM Notes WHERE OwnerId = 1 ORDER BY Id;

-- Show each note with its owner's username.
SELECT Users.Username, Notes.Title, Notes.Content
FROM Notes
JOIN Users ON Notes.OwnerId = Users.Id;
