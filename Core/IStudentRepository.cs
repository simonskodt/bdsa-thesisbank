namespace Core;

public interface IStudentRepository
{
    public Task<(Response, StudentDTO?)> ReadStudent(int studentID);
    public Task<(Response, int?)> ReadStudentIDByName(string studentName);
    public Task<(Response, ApplyDTO?)> Accept(int studentID, int thesisID);
    public Task<Response> RemoveAllPendings(int studentID);
}