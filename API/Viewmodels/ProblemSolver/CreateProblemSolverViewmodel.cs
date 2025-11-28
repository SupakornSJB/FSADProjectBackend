namespace FSADProjectBackend.Viewmodels.ProblemSolver;

public class CreateProblemSolverViewmodel
{
    public required string Name { get; set; }
    public required string ProfilePic { get; set; }
    public required string Description { get; set; }
    public required string[] InviteUserIds { get; set; }
}