using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Adapter.Tests.Harness
{
    internal delegate bool Converter<TIn, TOut>(TIn input, out TOut output);

    /// <summary>
    /// A difference the round trip is known to produce, and why.
    /// </summary>
    /// <remarks>
    /// <paramref name="Pattern"/> is matched against <see cref="Difference.Pattern"/>; <c>*</c>
    /// matches any run of characters.
    /// </remarks>
    internal sealed record ExpectedDifference(string Pattern, string Reason)
    {
        private Regex regex;

        public bool Matches(Difference difference)
        {
            regex ??= new Regex("^" + Regex.Escape(Pattern).Replace(@"\*", ".*") + "$");
            return regex.IsMatch(difference.Pattern);
        }
    }

    internal sealed class RoundTripResult<TAce>
    {
        public string Name { get; init; }
        public bool ForwardSucceeded { get; init; }
        public bool BackSucceeded { get; init; }
        public TAce Before { get; init; }
        public TAce After { get; init; }
        public List<Exception> Swallowed { get; init; }
        public List<Difference> Differences { get; init; }

        /// <summary>
        /// Fails the test unless both conversions succeeded without swallowing an exception and
        /// every difference is covered by <paramref name="expected"/>.
        /// </summary>
        public void AssertClean(params ExpectedDifference[] expected)
        {
            var unexpected = Differences.Where(d => !expected.Any(e => e.Matches(d))).ToList();

            if (ForwardSucceeded && BackSucceeded && Swallowed.Count == 0 && unexpected.Count == 0)
                return;

            var message = new StringBuilder();
            message.AppendLine($"{Name}: forward {(ForwardSucceeded ? "ok" : "FAILED")}, back {(BackSucceeded ? "ok" : "FAILED")}");

            foreach (var exception in Swallowed)
                message.AppendLine($"  swallowed: {SwallowedExceptions.Describe(exception)}");

            foreach (var difference in unexpected)
                message.AppendLine($"  unexpected: {difference}");

            Assert.Fail(message.ToString());
        }
    }

    internal static class RoundTrip
    {
        /// <summary>
        /// Converts ACE -> external -> ACE, recording swallowed exceptions and diffing the two ACE
        /// graphs.
        /// </summary>
        public static RoundTripResult<TAce> Run<TAce, TExternal>(string name, TAce before, Converter<TAce, TExternal> forward, Converter<TExternal, TAce> back)
        {
            var backSucceeded = false;
            TAce after = default;
            bool forwardSucceeded;
            List<Exception> swallowed;

            using (var watcher = new SwallowedExceptions())
            {
                forwardSucceeded = forward(before, out var external);
                if (forwardSucceeded)
                    backSucceeded = back(external, out after);

                swallowed = watcher.Exceptions;
            }

            return new RoundTripResult<TAce>
            {
                Name = name,
                ForwardSucceeded = forwardSucceeded,
                BackSucceeded = backSucceeded,
                Before = before,
                After = after,
                Swallowed = swallowed,
                Differences = backSucceeded ? Differ.Diff(before, after, name) : new List<Difference>(),
            };
        }
    }
}
