using System;

namespace MathSharp.Attributes
{
    public class ManualInlinesAttribute : Attribute
    {
        public string[] Methods;

        public ManualInlinesAttribute(params string[] methods)
        {
            Methods = methods ?? new string[0];
        }
    }
}
