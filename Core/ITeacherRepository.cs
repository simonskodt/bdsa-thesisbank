namespace Core;

public interface ITeacherRepository
{

    public Task<(Response, TeacherDTO)> ReadTeacher(int TeacherID);

    public Response Accept(int ThesisID);

    public Response Reject(int ThesisID);
    
}