using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace expenses_API.Models
{
    public class Entry
    {
        [Key] // used to idicate Id as a primary key
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsExpense { get; set; }
        public double Value { get; set; }

    }
}