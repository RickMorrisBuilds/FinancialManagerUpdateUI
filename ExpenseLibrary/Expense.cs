namespace ExpenseObject
{
    public class Expense
    {
        private string name;
        private string owner;
        private string id;
        private decimal amount;
        private string dueDate;
        private string issuer;
        private string status;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public string DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public string Issuer
        {
            get { return issuer; }
            set { issuer = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public Expense(string newName, string newOwner, string newId, decimal newAmount, string newDueDate, string newIssuer, string newStatus)
        {
            Name = newName;
            Owner = newOwner;
            Id = newId;
            Amount = newAmount;
            DueDate = newDueDate;
            Issuer = newIssuer;
            Status = newStatus;

        }


    }
}