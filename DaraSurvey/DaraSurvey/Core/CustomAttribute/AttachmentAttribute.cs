using System;
using System.Collections.Generic;
using System.Reflection;

namespace DaraSurvey.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AttachmentAttribute : Attribute
    {
        public readonly string Container;

        public readonly string Bucket;

        public AttachmentAttribute(string container, string bucket)
        {
            Container = container;
            Bucket = bucket;
        }
    }

    // --------------------------------------------

    public class AttachmentAttributeInfo
    {
        public AttachmentAttributeInfo(PropertyInfo property, string container, string bucket)
        {
            Property = property;
            Container = container;
            Bucket = bucket;
        }

        public PropertyInfo Property;

        public string Container;

        public string Bucket;

        // --------------------------------------------

        public static IEnumerable<AttachmentAttributeInfo> GetAttachmentAttributes(Type type)
        {
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            foreach (var prop in props)
            {
                var attr = (AttachmentAttribute)Attribute.GetCustomAttribute(prop, typeof(AttachmentAttribute), true);
                if (attr != null)
                    yield return new AttachmentAttributeInfo(prop, attr.Container, attr.Bucket);
            }
        }
    }
}
