namespace Core;

public interface ITeacherRepository
{
    public Task<(Response, TeacherDTO?)> ReadTeacher(int TeacherID);
    public Task<(Response, ApplyDTO?)> Accept(int StudentID, int ThesisID);

    public Task<(Response, int?)> ReadTeacherIDByName(string teacherName);
    /*Returns a list of ApplyDTO that shows all the students that
     have applied to thesis by the given Teacher (applies with status pending)*/
    public Task<IReadOnlyCollection<ApplyWithIDDTO>> ReadPendingStudentApplication(int TeacherID);

}