
namespace Identity.BLL.Interface.Data.Validation
{
    public class OperationDetails 
    {
        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

        public bool Equals(OperationDetails other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Succedeed == other.Succedeed && string.Equals(Message, other.Message) && string.Equals(Property, other.Property);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OperationDetails) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Succedeed.GetHashCode();
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Property != null ? Property.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
