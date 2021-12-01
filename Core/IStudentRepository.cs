namespace Core;

public interface IStudentRepository
{

    public Task<(Response, StudentDTO)> ReadStudent(int StudentID);

    public Task <(Response, ApplyDTO)> ApplyForThesis(int StudentID, int ThesisID);

    public Task <(Response, ApplyDTO)> Accept(int studentID, int ThesisID);

    public Task <Response> RemoveAllPendings(int StudentID);
    public Task <Response> RemoveRequest(int studentID, int ThesisID);
    
}