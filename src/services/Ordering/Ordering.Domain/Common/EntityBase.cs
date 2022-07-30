namespace Ordering.Domain.Common
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
