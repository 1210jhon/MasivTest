using System;

namespace Crosscuting.SeedWork.Domain
{
    public abstract class Entity
    {
        private int? _requestedHashCode;
        private int _id;
        public virtual int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public bool IsTransient()
        {
            return Id == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;
            if ((object)this == obj)
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            Entity entity = (Entity)obj;
            return !entity.IsTransient() && !this.IsTransient() && entity.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (this.IsTransient())
                return base.GetHashCode();
            if (!this._requestedHashCode.HasValue)
                this._requestedHashCode = new int?(this.Id.GetHashCode() ^ 31);
            return this._requestedHashCode.Value;
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return object.Equals((object)left, (object)null) ? object.Equals((object)right, (object)null) : left.Equals((object)right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public bool Annulled { get; set; }
        public DateTime DateRegister { get; set; }
        public string UserRegister { get; set; }
        public DateTime? DateModify { get; set; }
        public string UserModify { get; set; }

    }
}
