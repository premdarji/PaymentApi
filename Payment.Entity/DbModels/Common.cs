using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Entity.DbModels
{
    public class Common
    {
        public int Id { get; set; }

        public string Parameter { get; set; }
        public string Value { get; set; }

        public int LanguageId { get; set; }
    }
}
