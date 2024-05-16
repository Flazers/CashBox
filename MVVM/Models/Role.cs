using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class Role
{
    [Key]
    public int Id { get; set; }

    public string Role1 { get; set; } = null!;

    public virtual ICollection<UserInfo> UserInfos { get; set; } = [];
}
