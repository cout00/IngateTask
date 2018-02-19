using System;

namespace IngateTask.Core.CommandInterpreter
{

    /// <summary>
    /// атрибут показывает что команда без параметра
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ParametrelessAttribute : Attribute
    {
    }
}