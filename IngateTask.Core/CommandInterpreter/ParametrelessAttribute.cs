using System;

namespace IngateTask.Core.CommandInterpreter
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ParametrelessAttribute : Attribute
    {
    }
}