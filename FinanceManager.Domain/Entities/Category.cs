using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceManager.Domain.Enums;


namespace FinanceManager.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public TransactionType Type { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }

}
