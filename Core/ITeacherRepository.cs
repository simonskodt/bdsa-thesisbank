namespace Core;

public interface ITeacherRepository
{

    public Response Accept(int ThesisID);

    public Response Reject(int ThesisID);
    
}