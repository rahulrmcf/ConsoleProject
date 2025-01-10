using System;
using Microsoft.Extensions.Logging;

public class Logger
{
    private readonly ILogger<Logger> _logger;

    public Logger(ILogger<Logger> logger)
    {
        _logger = logger;
    }

    public void Log(string message, string username = null)
    {
        if (username != null)
        {
            _logger.LogInformation($"{message} {username}");
        }
        else
        {
            _logger.LogInformation($"{message}");
        }
    }

    public void Log(Exception exception, string customMessage = null)
    {
        if (customMessage != null)
        {
            _logger.LogError(exception, $"{customMessage} - {exception.Message}");
        }
        else
        {
            _logger.LogError(exception, $"{exception.Message}");
        }
    }
}