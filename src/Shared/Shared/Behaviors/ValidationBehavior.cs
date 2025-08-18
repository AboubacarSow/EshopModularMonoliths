using FluentValidation;
using MediatR;
using Shared.CQRS;

namespace Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
: IPipelineBehavior<TRequest, TResponse>
where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task
        .WhenAll(validators
                    .Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

        var errors = validationResults.Where(v => v.Errors.Any())
            .Select(e => e.Errors);
        if (errors.Any())
        {
            foreach (var error in errors)
                throw new ValidationException(error);
        }

        return await next(CancellationToken.None);//Why do we need to foward the paramter cancellationToken
    }
}
