using System.Collections.Generic;

namespace Core
{
    public interface IThesisRepository
    {
        ThesisDTO ReadThesis(int id);
        IReadOnlyCollection<ThesisDTO> ReadAll();
        IReadOnlyCollection<ThesisDTO> ReadRequested(int teacherID);
    }
}