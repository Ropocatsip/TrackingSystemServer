public class Thesis
{
    public ThesisStatus Status { get; set; } = new ThesisStatus();
    public string Topic { get; set; } = "";
    public AdvisorInfo AdvisorInfo { get; set; } = new AdvisorInfo();
    public CommitteeInfo CommitteeInfo { get; set; } = new CommitteeInfo();

}

public class ThesisStatus 
{
    // Id = 0, ยังไม่มีที่ปรึกษา
    // Id = 1, อยู่ระหว่างพิจารณา
    // Id = 2, รอสอบ Proposal ครั้งที่ 1
    // Id = 3, รอสอบ Proposal ครั้งที่ 2
    // Id = 4, รอสอบ Presentation ครั้งที่ 1
    // Id = 5, รอสอบ Presentation ครั้งที่ 2
    // Id = 6, รอตีพิมพ์
    // Id = 7, รอ Conference
    // Id = 8, สำเร็จ

    public int StatusId { get; set; }
    public string Desc { get; set; } = "";
    public string AppointmentDate { get; set; } = "";
}

public class AdvisorInfo
{
    public string FullName { get; set; } = "";
}

public class CommitteeInfo
{
    public string UserName { get; set; } = "";
    public string FullName { get; set; } = "";
}