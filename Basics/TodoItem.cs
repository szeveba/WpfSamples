using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basics
{
    class TodoItem
    {
        public string? Value { get; set; }
        public DateTime? DeadLine { get; set; }
        public DateTime? CompletionDate { get; set; }
        public override string ToString()
        {
            return Value ?? "NA";
        }
        public string ToCsv()
        {
            return $"{Value};{DeadLine};{CompletionDate}";
        }
        public static TodoItem FromCsv(string csv)
        {
            var splits = csv.Split(';');
            var op = new TodoItem()
            {
                Value = splits[0],
            };
            DateTime date;
            if (DateTime.TryParse(splits[1], out date)) op.DeadLine = date;
            if (DateTime.TryParse(splits[2], out date)) op.CompletionDate = date;
            return op;
        }
    }
}
