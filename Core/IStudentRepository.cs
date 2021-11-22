namespace Core;

public interface IStudentRepository
{
    public Response Apply(int ThesisID);

    public Response Accept(int ThesisID);

    public void RemoveAllPendings();
    public Response RemoveRequest(int ThesisID);
    
}