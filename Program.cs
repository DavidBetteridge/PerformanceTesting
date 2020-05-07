using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            var _ = BenchmarkRunner.Run<BenchmarkTests>();
        }
    }

    [MemoryDiagnoser]
    public class BenchmarkTests
    {
        private Service _service;

        public BenchmarkTests()
        {
            _service = new Service();
        }

        [Benchmark(Baseline =true)]
        public void Original()
        {
            _service.Original("simple");
            _service.Original("complex.simple");
        }

        [Benchmark()]
        public void TotallyLinq()
        {
            _service.TotallyLinq("simple");
            _service.TotallyLinq("complex.simple");
        }

        [Benchmark()]
        public void NoLinq()
        {
            _service.NoLinq("simple");
            _service.NoLinq("complex.simple");
        }

        [Benchmark()]
        public void NoCheck()
        {
            _service.NoCheck("simple");
            _service.NoCheck("complex.simple");
        }

        [Benchmark()]
        public void IndexOf()
        {
            _service.IndexOf("simple");
            _service.IndexOf("complex.simple");
        }

        [Benchmark()]
        public void Span1()
        {
            _service.Span1("simple");
            _service.Span1("complex.simple");
        }

        [Benchmark()]
        public void Span2()
        {
            _service.Span2("simple");
            _service.Span2("complex.simple");
        }
    }

    class Service
    {
        public string Original(string str)
        {
            var parts = str.Split('.');
            var section = str.Any(x => x.Equals('.')) ? parts[parts.Length - 1] : str;

            return section;
        }

        public string TotallyLinq(string str)
        {
            var parts = str.Split('.');
            var section = str.Any(x => x.Equals('.')) ? parts.Last() : str;

            return section;
        }

        public string NoLinq(string str)
        {
            var parts = str.Split('.');
            var section = parts.Length > 1 ? parts[parts.Length - 1] : str;

            return section;
        }

        public string NoCheck(string str)
        {
            var parts = str.Split('.');
            var section = parts[parts.Length - 1];

            return section;
        }

        public string IndexOf(string str)
        {
            var index = str.LastIndexOf('.');
            if (index < 0)
                return str;
            else
                return str[(index + 1)..];
        }

        public ReadOnlySpan<char> Span1(string str)
        {
            var index = str.LastIndexOf('.');
            if (index < 0)
                return str;
            else
                return str[(index + 1)..];
        }

        public ReadOnlySpan<char> Span2(string str)
        {
            var index = str.LastIndexOf('.');
            if (index < 0)
                return str;
            else
                return str.AsSpan()[(index + 1)..];
        }

    }

    public class Tests
    {
        [Theory]
        [InlineData("one", "one")]
        [InlineData("two.one", "one")]
        [InlineData("three.two.one", "one")]
        public void Original(string testData, string expectedResult)
        {
            var service = new Service();
            var actualResult = service.Original(testData);

            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("one", "one")]
        [InlineData("two.one", "one")]
        [InlineData("three.two.one", "one")]
        public void TotallyLinq(string testData, string expectedResult)
        {
            var service = new Service();
            var actualResult = service.TotallyLinq(testData);

            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("one", "one")]
        [InlineData("two.one", "one")]
        [InlineData("three.two.one", "one")]
        public void NoLinq(string testData, string expectedResult)
        {
            var service = new Service();
            var actualResult = service.NoLinq(testData);

            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("one", "one")]
        [InlineData("two.one", "one")]
        [InlineData("three.two.one", "one")]
        public void NoCheck(string testData, string expectedResult)
        {
            var service = new Service();
            var actualResult = service.NoCheck(testData);

            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("one", "one")]
        [InlineData("two.one", "one")]
        [InlineData("three.two.one", "one")]
        public void IndexOf(string testData, string expectedResult)
        {
            var service = new Service();
            var actualResult = service.IndexOf(testData);

            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("one", "one")]
        [InlineData("two.one", "one")]
        [InlineData("three.two.one", "one")]
        public void Span1(string testData, string expectedResult)
        {
            var service = new Service();
            var actualResult = service.Span1(testData);

            actualResult.ToString().Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("one", "one")]
        [InlineData("two.one", "one")]
        [InlineData("three.two.one", "one")]
        public void Span2(string testData, string expectedResult)
        {
            var service = new Service();
            var actualResult = service.Span2(testData);

            actualResult.ToString().Should().Be(expectedResult);
        }
    }
}
