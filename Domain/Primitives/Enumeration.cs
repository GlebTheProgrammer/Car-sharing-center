using System.Reflection;

namespace CarSharingApp.Domain.Primitives
{
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
        where TEnum : Enumeration<TEnum>
    {
        private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

        protected Enumeration(int value, string name) 
        {
            Value = value;
            Name = name;
        }

        public int Value { get; protected init; }

        public string Name { get; protected init; } = string.Empty;

        public static TEnum? FromValue(int value)
        {
            return Enumerations.TryGetValue(
                key: value,
                out TEnum? enumeration) ?
                enumeration :
                default;
        }

        public static TEnum? FromName(string name)
        {
            return Enumerations
                .Values
                .SingleOrDefault(e => e.Name == name);
        }

        public bool Equals(Enumeration<TEnum>? other)
        {
            if (other is null)
            {
                return false;
            }

            return GetType() == other.GetType() &&
                Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<TEnum> other &&
                Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        private static Dictionary<int, TEnum> CreateEnumerations()
        {
            var enumerationType = typeof(TEnum);

            var fieldForType = enumerationType
                .GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy)
                .Where(fieldInfo =>
                    enumerationType.IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldInfo =>
                    (TEnum)fieldInfo.GetValue(default)!);

            return fieldForType.ToDictionary(x => x.Value);
        }

        public static IEnumerable<string> GetPossibleEnumerations()
        {
            foreach (KeyValuePair<int, TEnum> keyValuePair in Enumerations)
            {
                yield return keyValuePair.Value.Name;
            }
        }
    }
}
