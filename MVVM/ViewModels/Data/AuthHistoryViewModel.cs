using Cashbox.MVVM.Models;

namespace Cashbox.MVVM.ViewModels.Data
{
    public class AuthHistoryViewModel(AuthHistory AuthHis)
    {
        private readonly AuthHistory _authHistory = AuthHis;

        public static Task<List<AuthHistoryViewModel>> GetAuthHistories() => AuthHistory.GetHistories();

        public int Id => _authHistory.Id;

        public DateTime Datetime => _authHistory.Datetime;

        public int UserId => _authHistory.UserId;
    }
}
