namespace Core;

public interface ITeacherRepository
{
    public Task<(Response, TeacherDTO?)> ReadTeacher(int TeacherID);
    public Task<(Response, ApplyDTO?)> Accept(int StudentID, int ThesisID);
    public Task<IReadOnlyCollection<ApplyWithIDDTO>> ReadApplicationsByTeacherID(int teacherID);
    public Task<IReadOnlyCollection<ApplyWithIDDTO>> ReadPendingStudentApplication(int TeacherID);

}