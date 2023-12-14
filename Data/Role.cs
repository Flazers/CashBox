using System;
using System.Collections.Generic;

namespace Cashbox.Data;

public partial class Role
{
    public int Id { get; set; }

    public string Role1 { get; set; } = null!;

    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
