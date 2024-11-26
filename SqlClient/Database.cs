using Microsoft.Data.SqlClient;
using SqlClient.Domain;
using SqlClient.SeedWork;

namespace SqlClient;

public class Database : IDatabase
{
    private readonly SqlConnection connection;

    public Database(string connectionString)
    {
        connection = new(connectionString);
        connection.Open();
    }

    public async ValueTask DisposeAsync() => await connection.DisposeAsync();

    public void Dispose() => connection.Dispose();

    public IEnumerable<MyNote> ReadAllNotes()
    {
        const string query = "SELECT Id, Note, Inserted FROM Notes";

        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read()) yield return new(
            reader.GetInt32(0), 
            reader.GetString(1), 
            reader.GetDateTimeOffset(2)
        );
    }

    public void InsertNote(string note)
    {
        const string query = "INSERT INTO Notes (Note, Inserted) VALUES (@Note, @Inserted)";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Note", note);
        command.Parameters.AddWithValue("@Inserted", DateTimeOffset.Now);

        command.ExecuteNonQuery();
    }

    public void UpdateNote(int id, string note)
    {
        const string query = "UPDATE Notes SET Note = @Note WHERE Id = @Id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Note", note);

        command.ExecuteNonQuery();
    }

    public void DeleteNoteWithConfirmation(int id)
    {
        const string query = "DELETE FROM Notes WHERE Id = @Id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }
}