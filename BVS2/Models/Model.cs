namespace BVS2.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class Video
    {
        public int VideoID { get; set; }
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public int TotalQuantity { get; set; }
        public int RentalDays { get; set; }
        public decimal RentalPrice { get; set; }
        public int QuantityIn { get; set; }
        public int QuantityOut { get; set; }
    }

    public class Rental
    {
        public int RentalID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = "";
        public string Phone { get; set; } = "";
        public int VideoID { get; set; }
        public string VideoTitle { get; set; } = "";
        public string Category { get; set; } = "";
        public DateTime RentalDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal RentalFee { get; set; }
        public decimal OverdueFee { get; set; }
        public decimal TotalFee { get; set; }
        public string Status { get; set; } = "";
        public int DaysOverdue { get; set; }
    }
}