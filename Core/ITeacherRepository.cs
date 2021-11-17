namespace Core;


public interface ITeacherRepository
{
    Response Accecpt(int thesisID);

    Response Reject(int thesisID);
}
