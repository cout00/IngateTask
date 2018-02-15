using System;

namespace IngateTask.Core.CommandInterpreter
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal sealed class ParametrelessAttribute : Attribute
    {
    }
}