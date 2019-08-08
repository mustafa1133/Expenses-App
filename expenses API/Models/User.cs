using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace expenses_API.Models
{
    public class User
    {
        [Key]// used to define a primary key for the table
        public int Id { get; set; }
        public string UserName{ get; set; }// table columns
        public string Password { get; set; }//table columns
    }
}