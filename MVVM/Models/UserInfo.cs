using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class UserInfo
{
    [Key]
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public int Salary { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
