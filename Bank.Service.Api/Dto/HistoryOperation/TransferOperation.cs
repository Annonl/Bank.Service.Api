using System.ComponentModel.DataAnnotations;

namespace Bank.Service.Api.Dto.HistoryOperation
{
    /// <summary>
    /// Операция по переводу между счетами.
    /// </summary>
    public class TransferOperation
    {
        /// <summary>
        /// Номер карты отправителя.
        /// </summary>
        [Required] 
        public string NumberCardFrom { get; set; }

        /// <summary>
        /// Номер карты получателя.
        /// </summary>
        [Required]
        public string NumberCardTo { get; set; }

        /// <summary>
        /// Сумма перевода.
        /// </summary>
        [Required]
        public decimal Amount { get; set; }
    }
}
