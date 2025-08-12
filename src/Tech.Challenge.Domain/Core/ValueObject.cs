using System;
using System.Collections.Generic;
using System.Linq;

namespace Tech.Challenge.Domain.Core;

public abstract class ValueObject<T> : IEquatable<T>
    where T : ValueObject<T>
{
    /// <summary>
    /// Deve ser sobrescrito na classe derivada para retornar os componentes que compõem o ValueObject.
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        return Equals(obj as T);
    }

    public bool Equals(T other)
    {
        if (other == null)
            return false;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        // XOR entre os hash codes dos componentes
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });
    }

    public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals((T)b);
    }

    public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        var components = GetEqualityComponents()
            .Select(c => c?.ToString() ?? string.Empty);

        return string.Join(";", components);
    }
}

