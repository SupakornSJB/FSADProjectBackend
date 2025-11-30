namespace FSADProjectBackend.Viewmodels.Project;

public class CreateProjectViewmodel
{
    public Models.Project Project { get; set; }
    public required string ProblemSolverId { get; set; }
    public string? RelatedProblemId { get; set; }
    public string? RelatedSolutionId { get; set; }
}