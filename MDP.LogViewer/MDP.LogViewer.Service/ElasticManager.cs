using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Transport;

namespace MDP.LogViewer.Service;

public class ElasticManager
{
    private readonly ElasticsearchClient _client;

    public ElasticManager(IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("ElkHost", "localhost")!;
        var settings = new ElasticsearchClientSettings(new Uri($"http://{host}:9200"))
            .Authentication(new BasicAuthentication("elastic", "changeme"));

        _client = new ElasticsearchClient(settings);
    }

    public async Task ClearAsync()
    {
        await _client.Indices.DeleteDataStreamAsync(new DeleteDataStreamRequest("logs-generic-default"));
    }
}