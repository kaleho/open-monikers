using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Open.Monikers
{
    [DataContract]
    public class RefId
        : IRefId
    {
        private readonly string _string;
        private readonly string _uri;

        public RefId(
            string name,
            RefId parentId = null)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                name.StartsWith(IRefId.NameSeparator) ||
                name.EndsWith(IRefId.NameSeparator))
            {
                throw new ArgumentException(
                    $"The name [{name}] is invalid or missing.");
            }

            ParentId = parentId;

            Name = name.ToLowerInvariant();

            _uri =
                ParentId != null
                    ? $"{ParentId.ToUri()}{IRefId.UriSeparator}{Name}"
                    : $"{IRefId.UriSeparator}{Name}";

            _string =
                ParentId != null
                    ? $"{ParentId}{IRefId.NameSeparator}{Name}"
                    : Name;
        }

        [DataMember(Order = 0)]
        public string Name { get; }

        [DataMember(Order = 1)]
        public RefId ParentId { get; }

        public static bool operator !=(
            RefId a,
            RefId b)
        {
            return !(a == b);
        }

        public static bool operator ==(
            RefId a,
            RefId b)
        {
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        public static TNameId Reparent<TNameId>(
            RefId refId,
            RefId parentId)
        {
            var names = new List<string>();

            var currentParent = refId.ParentId;

            names.Add(refId.Name);

            while (currentParent != null)
            {
                names.Add(currentParent.Name);

                currentParent = currentParent.ParentId;
            }

            var newNameId = parentId;

            for (var i = names.Count - 1; i >= 0; i--)
            {
                newNameId = new RefId(names[i], newNameId);
            }

            var returnValue =
                (TNameId)Activator.CreateInstance(
                    typeof(TNameId),
                    newNameId.Name,
                    newNameId.ParentId);

            return returnValue;
        }

        public static bool TryParseUri<TNameId>(
            string uri,
            out TNameId nameId)
        {
            var names =
                uri
                    .Split(IRefId.UriSeparator, StringSplitOptions.RemoveEmptyEntries)
                    .Reverse()
                    .ToArray();

            RefId newRefId = default;

            for (var i = names.Length - 1; i >= 0; i--)
            {
                newRefId = new RefId(names[i], newRefId);
            }

            try
            {
                nameId =
                    (TNameId)Activator.CreateInstance(
                        typeof(TNameId),
                        newRefId.Name,
                        newRefId.ParentId);
            }
            catch
            {
                nameId = default;
            }

            return newRefId != default;
        }

        public override bool Equals(
            object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((RefId)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ParentId, Name);
        }

        public override string ToString()
        {
            return _string;
        }

        public virtual string ToUri()
        {
            return _uri;
        }

        protected bool Equals(RefId other)
        {
            if (other == default)
            {
                return false;
            }

            return ToString() == other.ToString();
        }
    }
}