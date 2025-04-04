using System;
using System.Linq;
using SPG_Fachtheorie.Aufgabe1.Model;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;


namespace SPG_Fachtheorie.Aufgabe1.Services
{
    public class PaymentService
    {
        private readonly AppointmentContext _db;

        public PaymentService(AppointmentContext db)
        {
            _db = db;
        }

        public Payment CreatePayment(NewPaymentCommand cmd)
        {
            var cashDesk = _db.CashDesks.FirstOrDefault(cd => cd.Id == cmd.CashDeskId)
                ?? throw new PaymentServiceException("Cashdesk not found.");

            var employee = _db.Employees.FirstOrDefault(e => e.Id == cmd.EmployeeId)
                ?? throw new PaymentServiceException("Employee not found.");

            var existing = _db.Payments
                .FirstOrDefault(p => p.CashDesk.Id == cmd.CashDeskId && p.Confirmed == null);

            if (existing != null)
            {
                throw new PaymentServiceException("Open payment for cashdesk.");
            }

            if (cmd.PaymentType == PaymentTypes.CreditCard && employee is not Manager)
            {
                throw new PaymentServiceException("Insufficient rights to create a credit card payment.");
            }

            var payment = new Payment
            {
                CashDesk = cashDesk,
                Employee = employee,
                PaymentType = cmd.PaymentType,
                Created = DateTime.UtcNow,
                Confirmed = null,
                Items = new List<PaymentItem>()
            };

            _db.Payments.Add(payment);
            _db.SaveChanges();

            return payment;
        }

        public void ConfirmPayment(int paymentId)
        {
            var payment = _db.Payments.FirstOrDefault(p => p.Id == paymentId)
                ?? throw new PaymentServiceException("Payment not found.");

            if (payment.Confirmed != null)
            {
                throw new PaymentServiceException("Payment already confirmed.");
            }

            payment.Confirmed = DateTime.UtcNow;
            _db.SaveChanges();
        }

        public void AddPaymentItem(NewPaymentItemCommand cmd)
        {
            var payment = _db.Payments.FirstOrDefault(p => p.Id == cmd.PaymentId)
                ?? throw new PaymentServiceException("Payment not found.");

            if (payment.Confirmed != null)
            {
                throw new PaymentServiceException("Payment already confirmed.");
            }

            var item = new PaymentItem
            {
                ArticleName = cmd.ArticleName,
                Amount = cmd.Amount,
                Price = cmd.Price,
                Payment = payment
            };

            _db.PaymentItems.Add(item);
            _db.SaveChanges();
        }

        public void DeletePayment(int paymentId, bool deleteItems)
        {
            var payment = _db.Payments.FirstOrDefault(p => p.Id == paymentId)
                ?? throw new PaymentServiceException("Payment not found.");

            if (deleteItems && payment.Items != null)
            {
                _db.PaymentItems.RemoveRange(payment.Items);
            }

            _db.Payments.Remove(payment);
            _db.SaveChanges();
        }
    }
}
