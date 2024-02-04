using PersonalProject.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PersonalProject.InternalPortal.Models.Installers;

public class EditInstallerViewModel
{
    public IEnumerable<InstallerStatus>? InstallerStatuses { get; set; }
    public EditInstallerStatusViewModel Status { get; set; }
    public EditInstallerDetailsViewModel Detail { get; set; }
}
