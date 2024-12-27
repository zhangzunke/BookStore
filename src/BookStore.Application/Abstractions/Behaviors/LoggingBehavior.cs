using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Abstractions.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
        where TResponse : Result
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;
            try
            {
                _logger.LogInformation("Executing request {RequestName}", requestName);
                var result = await next();
                if (result.IsSuccess)
                {
                    _logger.LogInformation("Command {RequestName} processed successfully", requestName);
                }
                else 
                {
                    using (LogContext.PushProperty("Error", result.Error, true))
                    {
                        _logger.LogError("Request {RequestName} processed with error", requestName);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Command {RequestName} processing failed", requestName);
                throw;
            }
        }
    }
}
