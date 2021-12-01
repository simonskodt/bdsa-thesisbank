namespace Core;

public interface ITeacherRepository
{
    public Task<(Response, TeacherDTO)> ReadTeacher(int TeacherID);
    public Task<(Response, ApplyDTO)> Accept(int StudentID, int ThesisID);
    public Task<(Response, ApplyDTO)> Reject(int StudentID, int ThesisID);

    /*Returns a list of ApplyDTO that shows all the students that
     have applied to thesis by the given Teacher (applies with status pending)*/
    public Task<IReadOnlyCollection<ApplyDTO>> ReadStudentApplication(int TeacherID);

}