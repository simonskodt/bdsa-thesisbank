namespace Core;

public interface ITeacherRepository
{
    public Task<(Response, TeacherDTO?)> ReadTeacher(int TeacherID);
    public Task<(Response, ApplyDTO?)> Accept(int StudentID, int ThesisID);
    //Helper method for ReadPendingApplicationsByTeacherID()
    public Task<IReadOnlyCollection<ApplyDTOWithMinalThesis>> ReadApplicationsByTeacherID(int teacherID);
    public Task<IReadOnlyCollection<ApplyDTOWithMinalThesis>?> ReadPendingApplicationsByTeacherID(int TeacherID);
}