using Cashbox.Core;
using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class AdminMoneyLogViewModel(AdminMoneyLog adminml) : ViewModelBase
    {
        private readonly AdminMoneyLog _adminml = adminml;

        public static async Task<bool> CreateTransitMB(string action, double money) => await AdminMoneyLog.CreateTransitMB(action, money);
        public static async Task<bool> CreateTransitAward(string action, double money, int subuserid) => await AdminMoneyLog.CreateTransitAward(action, money, subuserid);
        public static async Task<bool> CreateTransitSalary(string action, double money, int subuserid) => await AdminMoneyLog.CreateTransitSalary(action, money, subuserid);
        public static async Task<List<AdminMoneyLogViewModel>> GetAllLog() => await AdminMoneyLog.GetAllLog();

        public int Id => _adminml.Id;

        public int UserId => _adminml.UserId;

        public DateTime Datetime => _adminml.Datetime;

        public string? DataString => _adminml.Datetime.ToString("dd/MM/yyyy HH:mm:ss");

        public string Action => _adminml.Action;

        public double Money => _adminml.Money;

        public int? SubUserId => _adminml.SubUserId;

        public virtual User? SubUser => _adminml.SubUser;
        public virtual User User => _adminml.User;
    }
}
