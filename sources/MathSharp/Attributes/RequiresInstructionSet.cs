using System;
using MathSharp.Attributes;

namespace MathSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequiresInstructionSetAttribute : Attribute
    {
        public InstructionSets[] InstructionSets;

        public RequiresInstructionSetAttribute(params InstructionSets[] instructionSets)
        {
            InstructionSets = instructionSets is null || instructionSets.Length == 0
                ? new[] { Attributes.InstructionSets.None } : instructionSets;
        }
    }
}
