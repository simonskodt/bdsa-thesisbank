namespace ThesisBank.Client.Service;
public interface IThesesService
{
    // Ought to be ThesisDTO I guess
        // Task<List<Thesis>> GetAllTheses();
        Task<Thesis> GetThesis(int id);

    /**
    * Extra
    * - Apply for thesis
    * - Confirm acceptance
    */
}