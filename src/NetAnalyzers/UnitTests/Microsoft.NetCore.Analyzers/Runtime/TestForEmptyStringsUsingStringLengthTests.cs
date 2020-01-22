// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using VerifyCS = Test.Utilities.CSharpCodeFixVerifier<
    Microsoft.NetCore.Analyzers.Runtime.TestForEmptyStringsUsingStringLengthAnalyzer,
    Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider>;

namespace Microsoft.NetCore.Analyzers.Runtime.UnitTests
{
    public class TestForEmptyStringsUsingStringLengthTests
    {
        #region Helper methods

        private DiagnosticResult CSharpResult(int line, int column)
            => VerifyCS.Diagnostic()
                .WithLocation(line, column);

        #endregion

        #region Diagnostic tests

        [Fact]
        public async Task CA1820StaticEqualsTestCSharp()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;

class C
{
    void Method()
    {
        string a = null;

        // equality with empty string
        string.Equals(a, """");
        string.Equals(a, """", StringComparison.CurrentCulture);
        string.Equals("""", a, StringComparison.Ordinal);

        // equality with string.Empty
        string.Equals(a, string.Empty);
        string.Equals(a, string.Empty, StringComparison.CurrentCulture);
        string.Equals(string.Empty, a, StringComparison.Ordinal);
    }
}
",
                CSharpResult(11, 9),
                CSharpResult(12, 9),
                CSharpResult(13, 9),
                CSharpResult(16, 9),
                CSharpResult(17, 9),
                CSharpResult(18, 9));
        }

        [Fact]
        public async Task CA1820InstanceEqualsTestCSharp()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;

class C
{
    void Method()
    {
        string a = null;

        // equality with empty string
        a.Equals("""");
        a.Equals("""", StringComparison.CurrentCulture);

        // equality with string.Empty
        a.Equals(string.Empty);
        a.Equals(string.Empty, StringComparison.CurrentCulture);
    }
}
",
                CSharpResult(11, 9),
                CSharpResult(12, 9),
                CSharpResult(15, 9),
                CSharpResult(16, 9));
        }

        [Fact]
        public async Task CA1820OperatorOverloadTestCSharp()
        {
            await VerifyCS.VerifyAnalyzerAsync(@"
using System;

class C
{
    void Method()
    {
        string a = null;
        if (a == """") { }
        if ("""" != a) { }
        if (a == string.Empty) { }
        if (string.Empty != a) { }
    }
}
",
                CSharpResult(9, 13),
                CSharpResult(10, 13),
                CSharpResult(11, 13),
                CSharpResult(12, 13));
        }

        #endregion
    }
}