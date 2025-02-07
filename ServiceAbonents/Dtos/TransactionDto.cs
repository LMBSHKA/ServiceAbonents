﻿using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Dtos
{
    public class TransactionDto
    {
        [Required]
        public Guid ClientId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
    }
}
