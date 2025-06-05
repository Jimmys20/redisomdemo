using Redis.OM;
using Redis.OM.Modeling;

namespace redisomdemo2;



[Document(StorageType = StorageType.Json, Prefixes = ["PlayerState"])]
public class PlayerStateEntity
{
  [RedisIdField, Indexed] public string UniquePlayerId { get; set; }
  //public DateTimeOffset StartTime { get; set; }
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

    provider.Connection.DropIndexAndAssociatedRecords(typeof(PlayerStateEntity));
    await provider.Connection.CreateIndexAsync(typeof(PlayerStateEntity));


    var playerStates = provider.RedisCollection<PlayerStateEntity>();

    var uniquePlayerId = "abc-234";
    //var startTime = new DateTimeOffset(2024, 11, 21, 12, 30, 00, new TimeSpan(1, 0, 0));

    await playerStates.InsertAsync(new PlayerStateEntity
    {
      UniquePlayerId = uniquePlayerId,
      //StartTime = startTime,
    });

    return;

    var player = await playerStates.FindByIdAsync(uniquePlayerId);

    //await players.SaveAsync();

    await playerStates.InsertAsync(new PlayerStateEntity
    {
      UniquePlayerId = uniquePlayerId,
      //StartTime = startTime,
    });

    var player2 = await playerStates.FindByIdAsync(uniquePlayerId);

    var x = await playerStates.ToListAsync();

    //Console.WriteLine(player.StartTime);
    //Console.WriteLine(player2.StartTime);
    //Console.WriteLine(player.StartTime.Offset == player2.StartTime.Offset);
  }
}
