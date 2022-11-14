// ReSharper disable MemberCanBePrivate.Local
using System;

namespace ZNotify.Utils;

internal static partial class Static
{
    /// <summary>
    /// Detect if we are running as part of a nUnit unit test.
    /// This is DIRTY and should only be used if absolutely necessary 
    /// as its usually a sign of bad design.
    /// </summary>    
    private static class UnitTestDetector
    {
        static UnitTestDetector()
        {
            foreach (var assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assem.FullName.ToLowerInvariant().StartsWith("nunit.framework")) continue;
                IsRunningFromNUnit = true;
                break;
            }
        }

        public static bool IsRunningFromNUnit;
    }
}