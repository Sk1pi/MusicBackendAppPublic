using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Interfaces.Services;

public interface IStudentCardVerificationService
{
    Task<Result<bool, Error>> VerifyStudentCardAsync(string studentCardId);
}