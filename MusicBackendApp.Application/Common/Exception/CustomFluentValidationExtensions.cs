using CSharpFunctionalExtensions;
using FluentValidation;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Exception;

public static class CustomFluentValidationExtensions
{
    public static IRuleBuilderOptionsConditions<T, TProperty> MustBeValueObject<T, TProperty, TValue, TError>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<TProperty, Result<TValue, TError>> factoryMethod) //Делегат, за доаомогою якого ми будемо перевіряти наше свойство 
        where TError : Error //Делегат, за доаомогою якого ми будемо перевіряти наше свойство 
        // будь-який тип, який використовується як TError для цього методу, повинен успадковуватися від твого базового класу Error
        // Зараз делегат - це фабрика, яка є у кожного valueobject
    {
        return ruleBuilder.Custom((value, context) =>
        {
            //Тут є свойства, які переходять з одної валіації в іншу, яка має інформацію про теперішню валідацію(через context)
            //Що це за свойство, які характеристики воно має?
            
            //Кожеш valuepbject у нього є factoryMethod, яки повертає Result, з типом цього TValueObject з помилкою 
            Result<TValue, TError> result = factoryMethod(value); //викликаємо factoryMethod (тобто, метод Create твого Value Object'а)
            
            if (result.IsSuccess) //Якщо успішно, то не трогаємо 
                return;
            
            context.AddFailure(result.Error.Serialize()); //Якщо є помилка 
        });
    }
}