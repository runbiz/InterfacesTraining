using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Interfaces.Entities
{
    [Table("Accounts")]
    public class Account
    {
        [Column("AccountId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Date created is required")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Account type is required")]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "Developer Id is required")]

        [ForeignKey(nameof(Developer))]
        public Guid DeveloperId { get; set; }
        [JsonIgnore]
        public Developer Developer { get; set; }
    }
}
