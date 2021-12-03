namespace Core;

public interface IStudentRepository
{
    public Task<(Response, StudentDTO?)> ReadStudent(int studentID);

    public Task<(Response, ApplyDTO?)> ApplyForThesis(int studentID, int thesisID);

    public Task<(Response, ApplyDTO?)> Accept(int studentID, int thesisID);

    public Task<Response> RemoveAllPendings(int studentID);

    public Task<Response> RemoveRequest(int thesisID, int studentID);
    
}