using System;
using Xunit;

namespace Lazy.Test
{
    public class SimpleLazyTest
    {
        private Random random = new Random();

        [Fact]
        public void NullFunctionTest()
        {
            bool? nullFunction() => null;

            var simpleLazy = LazyFactory.CreateSimpleLazy(nullFunction);

            Assert.Null(simpleLazy.Get);
        }

        [Fact]
        public void OrdinaryGetTest()
        {
            int function() => 21;
            
            var simpleLazy = LazyFactory.CreateSimpleLazy(function);

            Assert.Equal(21, simpleLazy.Get);
        }

        [Fact]
        public void SecondCalculatingTest()
        {
            int randomFunction() => random.Next(0, 100);

            var simpleLazy = LazyFactory.CreateSimpleLazy(randomFunction);

            var firstResult = simpleLazy.Get;
            var secondResult = simpleLazy.Get;

            Assert.Equal(firstResult, secondResult);
        }
    }
}
