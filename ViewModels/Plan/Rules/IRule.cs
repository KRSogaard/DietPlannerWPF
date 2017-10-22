namespace DietPlanner.ViewModels.Plan
{
    public interface IRule
    {
        bool IsViolated { get; }
        string ViolatedText { get; }
    }
}