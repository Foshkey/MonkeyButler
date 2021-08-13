using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using MonkeyButler.Abstractions.Data.Storage.Models.ImportExport;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace MonkeyButler.Data.Storage.Tests
{
    public class ImportExportAccessorTests
    {
        private readonly Mock<IConnectionMultiplexer> _connectionMultiplexer = new();
        private readonly Mock<IServer> _server = new();
        private readonly IDistributedCache _cache = TestUtility.CreateCache();

        public ImportExportAccessorTests()
        {
            _connectionMultiplexer.Setup(x => x.GetEndPoints(It.IsAny<bool>()))
                .Returns(new EndPoint[] { new IPEndPoint(IPAddress.Loopback, 1) });
            _connectionMultiplexer.Setup(x => x.GetServer(It.IsAny<EndPoint>(), It.IsAny<object>()))
                .Returns(_server.Object);
        }

        private ImportExportAccessor Build() => new(
            _connectionMultiplexer.Object,
            _cache);

        private void AddKeys(params RedisKey[] keys)
        {
            _server.Setup(x => x.Keys(
                It.IsAny<int>(),
                It.IsAny<RedisValue>(),
                It.IsAny<int>(),
                It.IsAny<long>(),
                It.IsAny<int>(),
                It.IsAny<CommandFlags>()))
                .Returns(keys);
        }

        [Fact]
        public async Task ShouldExport()
        {
            _cache.SetString("key1", "value1");
            _cache.SetString("key2", "value2");
            AddKeys("key1", "key2");

            var result = await Build().ExportAll();

            Assert.Equal(2, result.Export.Count);
            Assert.Contains(result.Export, kvp => kvp.Key == "key1" && kvp.Value == "value1");
            Assert.Contains(result.Export, kvp => kvp.Key == "key2" && kvp.Value == "value2");
        }

        [Fact]
        public async Task ExportShouldHandleNull()
        {
            var result = await Build().ExportAll();
            Assert.Equal(0, result.Export.Count);
        }

        [Fact]
        public async Task ExportShouldHandleMissingKeys()
        {
            AddKeys("key1", "key2");

            var result = await Build().ExportAll();

            Assert.Equal(2, result.Export.Count);
            Assert.Contains(result.Export, kvp => kvp.Key == "key1" && kvp.Value is null);
            Assert.Contains(result.Export, kvp => kvp.Key == "key2" && kvp.Value is null);
        }

        [Fact]
        public async Task ShouldHandleMassiveExport()
        {
            var count = 100000;
            var keys = new ConcurrentBag<RedisKey>();
            await Task.WhenAll(Enumerable.Range(0, count).Select(i =>
            {
                var key = $"key{i}";
                var value = $"value{i}";

                keys.Add(key);

                return _cache.SetStringAsync(key, value);
            }));
            AddKeys(keys.ToArray());

            var result = await Build().ExportAll();

            Assert.Equal(count, result.Export.Count);
        }

        [Fact]
        public async Task ShouldImport()
        {
            var data = new ImportQuery()
            {
                Import = new Dictionary<string, string>()
                {
                    ["key1"] = "value1",
                    ["key2"] = "value2"
                }
            };

            await Build().ImportAll(data);

            Assert.Equal("value1", _cache.GetString("key1"));
            Assert.Equal("value2", _cache.GetString("key2"));
        }

        [Fact]
        public async Task ImportShouldHandleNull()
        {
            var data = new ImportQuery();
            await Build().ImportAll(data);
        }

        [Fact]
        public async Task ImportShouldHandleNullEntries()
        {
            var data = new ImportQuery()
            {
                Import = new Dictionary<string, string>()
                {
                    ["key1"] = null,
                    ["key2"] = "value2"
                }
            };

            await Build().ImportAll(data);

            Assert.Null(_cache.GetString("key1"));
            Assert.Equal("value2", _cache.GetString("key2"));
        }

        [Fact]
        public async Task ShouldHandleMassiveImport()
        {
            var data = new ImportQuery()
            {
                Import = Enumerable.Range(0, 100000).ToDictionary(i => $"key{i}", i => $"value{i}")
            };

            await Build().ImportAll(data);
        }

        [Fact]
        public async Task DeleteShouldHandleMissingKeys()
        {
            AddKeys("key1", "key2");
            await Build().DeleteAll();
        }
    }
}
