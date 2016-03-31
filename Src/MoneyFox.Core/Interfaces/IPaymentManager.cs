using System.Threading.Tasks;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;

namespace MoneyFox.Core.Interfaces
{
    public interface IPaymentManager
    {
        void DeleteAssociatedPaymentsFromDatabase(Account account);

        Task<bool> CheckForRecurringPayment(Payment payment);

        void ClearPayments();

        void RemoveRecurringForPayments(RecurringPayment recurringPayment);
    }
}