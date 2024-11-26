using SqlClient.Domain;

namespace SqlClient.SeedWork;

public interface IDatabase : IDisposable, IAsyncDisposable
{
    IEnumerable<MyNote> ReadAllNotes();
    void InsertNote(string note);
    void UpdateNote(int id, string note);
    void DeleteNoteWithConfirmation(int id);
}