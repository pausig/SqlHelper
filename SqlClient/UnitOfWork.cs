using SqlClient.SeedWork;

namespace SqlClient;

public class UnitOfWork(IDatabase database) : IDisposable, IAsyncDisposable
{
    public void ReadAllNotes()
    {
        foreach (var note in database.ReadAllNotes())
        {
            Console.WriteLine($"Id: {note.Id}, Note: {note.Note}, Expiring Date: {note.Inserted}");
        }
    }

    public void InsertNote()
    {
        Console.Write("Enter note: ");
        var note = Console.ReadLine();
        database.InsertNote(note);
        Console.WriteLine("Note inserted successfully.");
    }

    public void UpdateNote()
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

    public void DeleteNoteWithConfirmation()
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

    public void Dispose() => database.Dispose();

    public async ValueTask DisposeAsync() => await database.DisposeAsync();
}