using System;
using System.Linq;
using System.Reflection;
using Humanizer;
using JsonApiDotNetCore.Models;
using str = JsonApiDotNetCore.Extensions.StringExtensions;


namespace JsonApiDotNetCore.Graph
{
    /// <summary>
    /// Provides an interface for formatting resource names by convention
    /// </summary>
    public interface IResourceNameFormatter
    {
        /// <summary>
        /// Get the publicly visible resource name from the internal type name
        /// </summary>
        string FormatResourceName(Type resourceType);
    }

    public class DefaultResourceNameFormatter : IResourceNameFormatter
    {
        /// <summary>
        /// Uses the internal type name to determine the external resource name.
        /// By default we us Humanizer for pluralization and then we dasherize the name.
        /// </summary>
        /// <example>
        /// <code>
        /// _default.FormatResourceName(typeof(TodoItem)).Dump(); 
        /// // > "todo-items"
        /// </code>
        /// </example>
        public string FormatResourceName(Type type)
        {
            try
            {
                // check the class definition first
                // [Resource("models"] public class Model : Identifiable { /* ... */ }
                if (type.GetCustomAttribute(typeof(ResourceAttribute)) is ResourceAttribute attribute)
                    return attribute.ResourceName;

                return str.Dasherize(type.Name.Pluralize());
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"Cannot define multiple {nameof(ResourceAttribute)}s on type '{type}'.", e);
            }
        }
    }
}
