namespace SeaweedFs
{
    public class Ttl
    {
        public static Ttl Infinite { get; } = new Ttl(null);
        private readonly string _ttl;
        private Ttl(string ttl)
        {
            this._ttl = ttl;
        }
        public bool IsInfinite => ReferenceEquals(this, Infinite) || string.IsNullOrEmpty(_ttl);
        public static Ttl FromMinutes(uint s) => s == 0 ? Infinite : new Ttl($"{s}m");
        public static Ttl FromHours(uint s) => s == 0 ? Infinite : new Ttl($"{s}h");
        public static Ttl FromDays(uint s) => s == 0 ? Infinite : new Ttl($"{s}d");
        public static Ttl FromWeeks(uint s) => s == 0 ? Infinite : new Ttl($"{s}w");
        public static Ttl FromMonths(uint s) => s == 0 ? Infinite : new Ttl($"{s}M");
        public static Ttl FromYears(uint s) => s == 0 ? Infinite : new Ttl($"{s}y");
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is Ttl t)
            {
                return this._ttl == t._ttl;
            }
            return false;
        }
        public override int GetHashCode() => this._ttl.GetHashCode();
        public override string ToString() => this._ttl.ToString();
    }
}
