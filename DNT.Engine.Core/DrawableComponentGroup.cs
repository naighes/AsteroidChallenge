using System;
using DNT.Engine.Core.Validation;

namespace DNT.Engine.Core
{
    public class DrawableComponentGroup : IEquatable<DrawableComponentGroup>
    {
        private readonly String _name;
        private readonly Int32 _sortOrder;

        internal DrawableComponentGroup(String name, Int32 sortOrder)
        {
            Verify.That(name).Named("name").IsNotNullOrWhiteSpace();
            _name = name;
            _sortOrder = sortOrder;
        }

        internal String Name
        {
            get { return _name; }
        }

        internal Int32 SortOrder
        {
            get { return _sortOrder; }
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return (obj is DrawableComponentGroup) && Equals((DrawableComponentGroup) obj);
        }

        public Boolean Equals(DrawableComponentGroup other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return ReferenceEquals(this, other) || Equals(other._name, _name);
        }

        public override Int32 GetHashCode()
        {
            return _name.GetHashCode();
        }

        public static Boolean operator ==(DrawableComponentGroup left, DrawableComponentGroup right)
        {
            return Equals(left, right);
        }

        public static Boolean operator !=(DrawableComponentGroup left, DrawableComponentGroup right)
        {
            return !Equals(left, right);
        }
    }
}