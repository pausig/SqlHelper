using SqlClient;

const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=ToDoList;Integrated Security=True;TrustServerCertificate=True";

using var database = new Database(connectionString);
using var unitOfWork = new UnitOfWork(database);

while (true)
{
    Console.WriteLine("Choose an operation: 1) Read All 2) Insert Note 3) Update Note 4) Delete Note 5) Exit");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            unitOfWork.ReadAllNotes();
            break;
        case "2":
            unitOfWork.InsertNote();
            break;
        case "3":
            unitOfWork.UpdateNote();
            break;
        case "4":
            unitOfWork.DeleteNoteWithConfirmation();
            break;
        case "5":
            return;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}