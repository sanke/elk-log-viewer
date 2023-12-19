using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace MDP.LogViewer.Service.Controllers;

[ApiController]
public partial class FileController : ControllerBase
{
    private readonly ElasticManager _elasticManager;
    private readonly string _host;
    public const int DateLength = 8;

    public FileController(IConfiguration configuration, ElasticManager elasticManager)
    {
        _elasticManager = elasticManager;
        _host = configuration.GetValue<string>("ServiceHost", "localhost")!;
    }

    [HttpPost]
    [Route("/api/v1/files/upload-text")]
    public async Task<IActionResult> UploadText(string text)
    {
        using var client = new TcpClient(_host, 50000);
        var bytes = Encoding.UTF8.GetBytes(text);
        await client.GetStream().WriteAsync(bytes, 0, bytes.Length);
        return Ok();
    }
    
    [HttpDelete]
    [Route("/api/v1/clear")]
    public async Task<IActionResult> Clear()
    {
        await _elasticManager.ClearAsync();
        return Ok();
    }

    [HttpPost]
    [Route("/api/v1/files/upload")]
    public async Task<IActionResult> Upload(IFormFile[] files)
    {
        using var client = new TcpClient(_host, 50000);
        
        foreach (IFormFile file in files)
        {
            StreamReader sr = new StreamReader(file.OpenReadStream());

            while (sr.Peek() >= 0)
            {
                var line = await sr.ReadLineAsync();

                if (line == null)
                {
                    continue;
                }
                
                Regex regex = TimeRegex();
                if (regex.IsMatch(line))
                {
                    var name = Path.GetFileNameWithoutExtension(file.FileName);

                    Regex dateRegex = DateRegex();
                    var match = dateRegex.Match(name);
                    if (!match.Success)
                    {
                        continue;
                    }
                    
                    string dateStr = match.Value;
                    DateOnly date;

                    try
                    {
                        date = DateOnly.ParseExact(dateStr, "yyyyMMdd");
                    } 
                    catch (FormatException)
                    {
                        continue;
                    }

                    line = $"{date.ToString("yyyy-MM-dd")} {line}";
                }
                
                var bytes = Encoding.UTF8.GetBytes(line + "\n");
                await client.GetStream().WriteAsync(bytes, 0, bytes.Length);
            }
        }
        
        return Ok();
    }

    [GeneratedRegex(@"^\d{2}:\d{2}:\d{2}")]
    private static partial Regex TimeRegex();
    
    [GeneratedRegex(@"\d{8}")]
    private static partial Regex DateRegex();
}