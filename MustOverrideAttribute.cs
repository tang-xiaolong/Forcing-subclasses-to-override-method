using System;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class MustOverrideAttribute : Attribute
{
    public string MethodName { get; private set; }

    public MustOverrideAttribute(string methodName)
    {
        MethodName = methodName;
    }
}