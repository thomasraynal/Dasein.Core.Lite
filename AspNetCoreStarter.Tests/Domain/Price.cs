using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarter.Tests.Domain
{

    public class Price : IPrice
    {
        private readonly string _asset;
        private readonly double _value;

        public Price(string asset, double value)
        {
            _asset = asset;
            _value = value;
        }

        public string Asset => _asset;

        public double Value => _value;

        public IEnumerable<string> GetCacheInvalidationTags()
        {
            yield return _asset;
        }

        public int GetCacheKey()
        {
            return _asset.GetHashCode();
        }
    }
}
