using Redis.OM;
using Redis.OM.Modeling;

namespace redisomdemo;

[Document(StorageType = StorageType.Json, Prefixes = ["Player"])]
public class PlayerEntity
{
  [RedisIdField, Indexed] public string UniquePlayerId { get; set; }
}

[Document(StorageType = StorageType.Json, Prefixes = ["PlayerState"])]
public class PlayerStateEntity
{
  [RedisIdField, Indexed] public string UniquePlayerId { get; set; }
}

internal class Program
{
  static async Task Main(string[] args)
  {
    var redisConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
    {
      EndPoints = { "192.168.100.4:6379" },
      User = null,
      Password = null,
      AllowAdmin = false,
      Ssl = false,
    };

    var provider = new RedisConnectionProvider(redisConfigurationOptions);

    provider.Connection.DropIndexAndAssociatedRecords(typeof(PlayerEntity));
    await provider.Connection.CreateIndexAsync(typeof(PlayerEntity));

    provider.Connection.DropIndexAndAssociatedRecords(typeof(PlayerStateEntity));
    await provider.Connection.CreateIndexAsync(typeof(PlayerStateEntity));

    var players = provider.RedisCollection<PlayerEntity>();
    var playerStates = provider.RedisCollection<PlayerStateEntity>();

    await players.InsertAsync(new PlayerEntity
    {
      UniquePlayerId = Guid.NewGuid().ToString(),
    });

    var playerList = await players.ToListAsync();

    await playerStates.InsertAsync(new PlayerStateEntity
    {
      UniquePlayerId = Guid.NewGuid().ToString(),
    });

    var playerList2 = await players.ToListAsync();

    Console.WriteLine(playerList2.Count); // Should be 1
  }
}
