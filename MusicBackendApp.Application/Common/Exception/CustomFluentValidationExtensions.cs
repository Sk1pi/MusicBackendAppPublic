using CSharpFunctionalExtensions;
using FluentValidation;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Exception;

public static class CustomFluentValidationExtensions
{
    public static IRuleBuilderOptionsConditions<T, TProperty> MustBeValueObject<T, TProperty, TValue, TError>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<TProperty, Result<TValue, TError>> factoryMethod) 
        where TError : Error 
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValue, TError> result = factoryMethod(value); 
            
            if (result.IsSuccess) 
                return;
            
            context.AddFailure(result.Error.Serialize());
        });
    }
}