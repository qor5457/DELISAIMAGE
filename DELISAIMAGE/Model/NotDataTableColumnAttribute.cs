using System;

namespace DELISAIMAGE.Model
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NotDataTableColumnAttribute : Attribute
    {
    }
}