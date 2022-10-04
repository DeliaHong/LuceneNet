using Lucene.Logic.query;
using Lucene.Logic.Services;
using Lucene_net;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

IServiceCollection serviceCollection = new ServiceCollection();
Register.ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();

using (IServiceScope scope = serviceProvider.CreateScope())
{
    var newsService = serviceProvider.GetRequiredService<NewsService>();
    TestTime();
    scope.Dispose();
}

void TestTime()
{
    var newsService = serviceProvider.GetRequiredService<NewsService>();
    Console.WriteLine("請輸入欲查詢字串:");
    string query = Console.ReadLine();
    GetNewsQuery getNewsQuery = new GetNewsQuery()
    { 
        Title = query,
        Content = query,
        Description = query
    };

    Stopwatch stopWatch = new Stopwatch();

    stopWatch.Start();
    newsService.Create();
    newsService.SearchByIndex(query);
    stopWatch.Stop();
    Console.WriteLine($"Lucene第1次搜尋【{stopWatch.ElapsedMilliseconds}】秒");

    stopWatch.Restart();
    newsService.SearchBySql(getNewsQuery);
    stopWatch.Stop();
    Console.WriteLine($"SQL語法第1次搜尋【{stopWatch.ElapsedMilliseconds}】秒");


    for (int i = 2; i < 5; i++)
    {
        stopWatch.Restart();
        newsService.SearchByIndex(query);
        stopWatch.Stop();
        Console.WriteLine($"Lucene第{i}次搜尋【{stopWatch.ElapsedMilliseconds}】秒");

        stopWatch.Restart();
        newsService.SearchBySql(getNewsQuery);
        stopWatch.Stop();
        Console.WriteLine($"SQL語法第1次搜尋【{stopWatch.ElapsedMilliseconds}】秒");
    }
}
