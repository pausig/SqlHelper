using System.Data;
using Microsoft.Data.SqlClient;
using SqlClient.Domain;
using SqlClient.SeedWork;

namespace SqlClient;

public class Database(string connectionString) : IDatabase
{
    private readonly SqlConnection connection = new(connectionString);

    public IEnumerable<MyNote> Read()
    {
        const string query = "SELECT Id, Note, Inserted FROM Notes";

        connection.Open();
        
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            yield return new(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetDateTimeOffset(2)
            );
        }
        
        connection.Close();
    }

    public void Insert(string note)
    {
        const string query = "INSERT INTO Notes (Note, Inserted) VALUES (@Note, @Inserted)";

        connection.Open();
        
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Note", note);
        command.Parameters.AddWithValue("@Inserted", DateTimeOffset.Now);

        command.ExecuteNonQuery();
        
        connection.Close();
    }

    public void Update(int id, string note)
    {
        const string query = "UPDATE Notes SET Note = @Note WHERE Id = @Id";

        connection.Open();
        
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Note", note);

        command.ExecuteNonQuery();

        connection.Close();
    }

    public void Delete(int id)
    {
        const string query = "DELETE FROM Notes WHERE Id = @Id";

        connection.Open();

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
        
        connection.Close();
    }

    public void MassiveInsert(ICollection<string> notes)
    {
        const string query = "INSERT INTO Notes (Note, Inserted) VALUES (@Note, @Inserted)";

        connection.Open();
        var time = DateTimeOffset.Now;
        
        var transaction = connection.BeginTransaction();
        using var command = new SqlCommand(query, connection,transaction);
        
        try
        {
            command.Parameters.Add("@Note", SqlDbType.NVarChar);
            command.Parameters.Add("@Inserted", SqlDbType.DateTimeOffset);
            foreach (var note in notes)
            {
                command.Parameters["@Note"].Value = note;
                command.Parameters["@Inserted"].Value = time;
                command.ExecuteNonQuery();
            }
            
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
        }
       
        connection.Close();
        
    }

    public async ValueTask DisposeAsync() => await connection.DisposeAsync();

    public void Dispose() => connection.Dispose();
}