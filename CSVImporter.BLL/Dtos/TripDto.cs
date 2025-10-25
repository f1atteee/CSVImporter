namespace CSVImporter.BLL.Dtos
{
    public class TripDto
    {
        public DateTime TpepPickupDatetime { get; set; }
        public DateTime TpepDropoffDatetime { get; set; }
        public short PassengerCount { get; set; }
        public double TripDistance { get; set; }
        public string StoreAndFwdFlag { get; set; } = string.Empty;
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public decimal FareAmount { get; set; }
        public decimal TipAmount { get; set; }
    }
}