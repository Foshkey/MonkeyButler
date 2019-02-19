using System;

namespace MonkeyButler.XivApi.Attributes
{
    /// <summary>
    /// An attribute to represent the XIVAPI name of a field or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ApiNameAttribute : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The XIVAPI name of the field or property.</param>
        public ApiNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The XIVAPI name of the field or property.
        /// </summary>
        public string Name { get; }
    }
}
