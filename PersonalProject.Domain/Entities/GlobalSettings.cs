namespace PersonalProject.Domain.Entities;
public class GlobalSettings
{
    public int Id { get; set; }
    public int NextAppNumber { get; set; } = 10000000;
    public int NextInstallerNumber { get; set; }
}