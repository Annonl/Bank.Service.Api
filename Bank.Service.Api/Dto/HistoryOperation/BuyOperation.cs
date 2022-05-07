using System.ComponentModel.DataAnnotations;

namespace Bank.Service.Api.Dto.HistoryOperation
{
    /// <summary>
    /// Данные для покупки.
    /// </summary>
    public class BuyOperation
    {
        /// <summary>
        /// Номер карты.
        /// </summary>
        [Required]
        public string NumberCard { get; set; }

        /// <summary>
        /// Окончание действия счёта.
        /// </summary>
        [Required]
        public DateTime DataEnd { get; set; }

        /// <summary>
        /// Код проверки подлинности карты.
        /// </summary>
        [Required]
        public int Cvc { get; set; }

        /// <summary>
        /// Количество денег.
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// Название операции.
        /// </summary>
        [Required]
        public string NameOperation { get; set; }

    }
}
