using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Services
{
    public class NewPaymentItemCommand
    {
        [Required]
        public string ArticleName { get; set; }

        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int PaymentId { get; set; }
    }
}
