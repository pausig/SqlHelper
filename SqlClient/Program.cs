using SqlClient;
using SqlClient.SeedWork;

const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ToDoList;Integrated Security=True;TrustServerCertificate=True";
IDatabase database = new Database(connectionString);

while (true)
{
    Console.WriteLine("Choose an operation: 1) Read All 2) Insert Note 3) Update Note 4) Delete Note 5) Exit");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ReadAllNotes();
            break;
        case "2":
            InsertNote();
            break;
        case "3":
            UpdateNote();
            break;
        case "4":
            DeleteNoteWithConfirmation();
            break;
        case "5":
            return;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

void ReadAllNotes()
{
    foreach (var note in database.ReadAllNotes())
    {
        Console.WriteLine($"Id: {note.Id}, Note: {note.Note}, Expiring Date: {note.Inserted}");
    }
}

void InsertNote()
{
    Console.Write("Enter note: ");
    var note = Console.ReadLine();
    database.InsertNote(note);
    Console.WriteLine("Note inserted successfully.");
}

void UpdateNote()
{
    Console.Write("Enter note Id to update: ");
    if (int.TryParse(Console.ReadLine(), out var id))
    {
        Console.Write("Enter new note: ");
        var note = Console.ReadLine();
        database.UpdateNote(id, note);
        Console.WriteLine("Note updated successfully.");
    }
    else
    {
        Console.WriteLine("Invalid Id. Please enter a valid integer.");
    }
}

void DeleteNoteWithConfirmation()
{
    Console.Write("Enter note Id to delete: ");
    if (int.TryParse(Console.ReadLine(), out var id))
    {
        Console.Write("Are you sure you want to delete this note? (yes/no): ");
        var confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "yes")
        {
            database.DeleteNoteWithConfirmation(id);
            Console.WriteLine("Note deleted successfully.");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }
    else
    {
        Console.WriteLine("Invalid Id. Please enter a valid integer.");
    }
}