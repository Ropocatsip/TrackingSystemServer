namespace server.Models;

public class AdvisoryRequest
{
    public string ThesisTopic { get; set; } = "";
    public string StudentId { get; set; } = "";
    public string StudentName { get; set; } = "";
    public string SubmitDate { get; set; } = "";
    public string UpdateDate { get; set; } = "";
    public ThesisStatus ThesisStatus { get; set; } = new ThesisStatus();
}