using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class TransactionObject
    {
        private DateTime _createdOn = DateTime.MinValue;

        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Amount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { 
            get
            {
                return (_createdOn == DateTime.MinValue) ? DateTime.Now : _createdOn;
            }

            set 
            {
                _createdOn = value;
            } 
        }
        [Required]
        public string Type { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual UserObject User { get; set; }

    }
}
