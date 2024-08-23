namespace LibrarySystem
{
    public class Fine : EntityBase
    {
        private Guid _idLoan;
        private double _fineAmount;
        private bool _wasPayed = false;

        public Guid IdLoan
        {
            get => _idLoan;
            set => _idLoan = value;
        }

        public double FineAmount
        {
            get => _fineAmount;
            set => _fineAmount = value;
        }

        public bool WasPayed
        {
            get => _wasPayed;
            set => _wasPayed = value;
        }
    }
}