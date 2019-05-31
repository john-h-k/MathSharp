using System;
using System.Diagnostics;

namespace MathSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class UsesInstructionSetAttribute : Attribute
    {
        public InstructionSets[] InstructionSets;

        public UsesInstructionSetAttribute(params InstructionSets[] instructionSets)
        {
            InstructionSets = instructionSets;
        }
    }
}