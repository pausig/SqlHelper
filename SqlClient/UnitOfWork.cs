using Microsoft.Data.SqlClient;
using SqlClient.SeedWork;
using static System.Console;

namespace SqlClient;

public class UnitOfWork(IDatabase database) : IDisposable, IAsyncDisposable
{
    public void Work()
    {
        void InternalWork()
        {
            while (true)
            {
                WriteLine("Choose an operation: 1) Read All 2) Insert Note 3) Update Note 4) Delete Note 5) Exit");
                var choice = ReadLine();

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
                        WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        try
        {
            InternalWork();
        }
        catch (SqlException exception)
        {
            WriteLine("An error occurred while executing the database operation. {0}", exception);
        }
        catch (Exception e)
        {
            WriteLine("An error occurred while executing the operation. {0}", e);
            throw;
        }
    }

    private void ReadAllNotes()
    {
        foreach (var note in database.ReadAllNotes())
        {
            WriteLine($"Id: {note.Id}, Note: {note.Note}, Expiring Date: {note.Inserted}");
        }
    }

    private void InsertNote()
    {
        Write("Enter note: ");
        var note = ReadLine();
        
        if (string.IsNullOrEmpty(note))
        {
            WriteLine("Note cannot be noll or empty.");
            return;
        }
        
        database.InsertNote(note);
        WriteLine("Note inserted successfully.");
    }

    private void UpdateNote()
    {
        Write("Enter note Id to update: ");
        if (int.TryParse(ReadLine(), out var id))
        {
            Write("Enter new note: ");
            
            var note = ReadLine();
            if (string.IsNullOrEmpty(note))
            {
                WriteLine("Note cannot be noll or empty.");
                return;
            }
            
            database.UpdateNote(id, note);
            WriteLine("Note updated successfully.");
        }
        else
        {
            WriteLine("Invalid Id. Please enter a valid integer.");
        }
    }

    private void DeleteNoteWithConfirmation()
    {
        Write("Enter note Id to delete: ");
        if (int.TryParse(ReadLine(), out var id))
        {
            Write("Are you sure you want to delete this note? (yes/no): ");
            var confirmation = ReadLine();

            if (confirmation?.ToLower() == "yes")
            {
                database.DeleteNoteWithConfirmation(id);
                WriteLine("Note deleted successfully.");
            }
            else
            {
                WriteLine("Deletion cancelled.");
            }
        }
        else
        {
            WriteLine("Invalid Id. Please enter a valid integer.");
        }
    }

    public void Dispose() => database.Dispose();

    public async ValueTask DisposeAsync() => await database.DisposeAsync();
}