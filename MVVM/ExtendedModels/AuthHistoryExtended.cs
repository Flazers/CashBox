using Cashbox.MVVM.ViewModels.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json.Nodes;

namespace Cashbox.MVVM.Models
{
    public partial class AuthHistory
    {
        public AuthHistory() { }

        public static async Task<AuthHistoryViewModel?> NewAuthUser()
        {
            DateTime dateTime = DateTime.Now;
            HttpClient client = new();
            try
            {
                HttpResponseMessage? response = await client.GetAsync("https://timeapi.io/api/TimeZone/zone?timeZone=Europe/Saratov");

                if (!response.IsSuccessStatusCode)
                    return CreateNewAuthDataTime(dateTime) ? new AuthHistoryViewModel(CashBoxDataContext.Context.AuthHistories.FirstOrDefault(x => x.Datetime == dateTime)!) : null;

                var resultJSON = JsonNode.Parse(await response.Content.ReadAsStringAsync());
                string? dataSTR = resultJSON?["currentLocalTime"]?.ToString();
                if (dataSTR != null)
                    dateTime = DateTime.Parse(dataSTR);
            }
            catch (HttpRequestException)
            {
                // нет интернета
                return CreateNewAuthDataTime(dateTime) ? new AuthHistoryViewModel(CashBoxDataContext.Context.AuthHistories.FirstOrDefault(x => x.Datetime == dateTime)!) : null;
            }
            return CreateNewAuthDataTime(dateTime) ? new AuthHistoryViewModel(CashBoxDataContext.Context.AuthHistories.FirstOrDefault(x => x.Datetime == dateTime)!) : null;
        }

        public static bool CreateNewAuthDataTime(DateTime dataTime)
        {
            UserViewModel user = UserViewModel.GetCurrentUser()!;
            try
            {
                CashBoxDataContext.Context.Add(new AuthHistory()
                {
                    UserId = user.Id,
                    Datetime = dataTime,
                });
                var authCurrentUserHistory = CashBoxDataContext.Context.AuthHistories.Where(x => x.UserId == user.Id);
                if (authCurrentUserHistory.Count() >= 9)
                    CashBoxDataContext.Context.AuthHistories.Remove(authCurrentUserHistory.FirstOrDefault()!);
                CashBoxDataContext.Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async static Task<List<AuthHistoryViewModel>> GetHistories() => await CashBoxDataContext.Context.AuthHistories.Select(s => new AuthHistoryViewModel(s)).ToListAsync();

    }
}
