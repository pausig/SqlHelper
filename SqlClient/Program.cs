using Microsoft.Data.SqlClient;

const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ToDoList;Integrated Security=True;TrustServerCertificate=True";

using var connection = new SqlConnection(connectionString);
connection.Open();

while (true)
{
    Console.WriteLine("Choose an operation: 1) Read All 2) Insert Note 3) Update Note 4) Delete Note 5) Exit");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ReadAllNotes(connection);
            break;
        case "2":
            InsertNote(connection);
            break;
        case "3":
            UpdateNote(connection);
            break;
        case "4":
            DeleteNoteWithConfirmation(connection);
            break;
        case "5":
            return;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

void ReadAllNotes(SqlConnection connection)
{
    const string query = "SELECT Id, Note, Inserted FROM Notes";
    using var command = new SqlCommand(query, connection);
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"Id: {reader["Id"]}, Note: {reader["Note"]}, Expiring Date: {reader["Inserted"]}");
    }
}

void InsertNote(SqlConnection connection)
{
    Console.Write("Enter note: ");
    var note = Console.ReadLine();

    const string query = "INSERT INTO Notes (Note, Inserted) VALUES (@Note, @Inserted)";
    using var command = new SqlCommand(query, connection);
    command.Parameters.AddWithValue("@Note", note);
    command.Parameters.AddWithValue("@Inserted", DateTimeOffset.Now);
    command.ExecuteNonQuery();
    Console.WriteLine("Note inserted successfully.");
}

void UpdateNote(SqlConnection connection)
{
    Console.Write("Enter note Id to update: ");
    if (int.TryParse(Console.ReadLine(), out var id))
    {
        // Use the parsed integer id
        Console.Write("Enter new note: ");
        var note = Console.ReadLine();

        const string query = "UPDATE Notes SET Note = @Note WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Note", note);
        command.ExecuteNonQuery();
        Console.WriteLine("Note updated successfully.");
    }
    else
    {
        Console.WriteLine("Invalid Id. Please enter a valid integer.");
    }
}

void DeleteNoteWithConfirmation(SqlConnection connection)
{
    Console.Write("Enter note Id to delete: ");
    if (int.TryParse(Console.ReadLine(), out var id))
    {
        Console.Write("Are you sure you want to delete this note? (yes/no): ");
        var confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "yes")
        {
            const string query = "DELETE FROM Notes WHERE Id = @Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
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