using System;
using System.Collections.Generic;

namespace Cashbox.Data;

public partial class UserInfo
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
