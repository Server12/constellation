using System;
using UnityEngine;

namespace Constellation.Module_1.Data
{
    [Serializable]
    public class StarsPairData : IEquatable<StarsPairData>, IEquatable<int>
    {
        [SerializeField] private int from;
        [SerializeField] private int to;

        public int From => from;

        public int To => to;

        public int Id => GetId(from, to);

        public static int GetId(int from, int to)
        {
            return from ^ to;
        }
        
        public bool Compare(int fromId, int toId)
        {
            return (From == fromId && To == toId) || (From == toId && To == fromId);
        }
        
        public bool Equals(StarsPairData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return from == other.from && to == other.to;
        }

        public bool Equals(int other)
        {
            return from == other || to == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StarsPairData)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(from, to);
        }
    }
}