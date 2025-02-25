using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansSamples.SemanticKernelAgents;

public static class ArrayExtensions
{
    public static T[] TakeLast<T>(this T[] source, int n)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Can't be negative");
        }

        if (n > source.Length)
        {
            n = source.Length;
        }

        var target = new T[n];
        Array.Copy(source, source.Length - n, target, 0, n);
        return target;
    }
}
