namespace Core;

public interface IStudentRepository
{

    public Task<(Response, StudentDTO)> ReadStudent(int StudentID);

    public Task <(Response, ApplyDTO)> ApplyForThesis(int StudentID, int ThesisID);

    public Response Accept(int ThesisID);

    public void RemoveAllPendings();
    public Response RemoveRequest(int ThesisID);
    
}