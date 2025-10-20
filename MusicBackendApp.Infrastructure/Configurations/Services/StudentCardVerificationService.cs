using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using MusicBackendApp.Application.Common.Interfaces.Services;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Infrastructure.Configurations.Services;

public class StudentCardVerificationService : IStudentCardVerificationService
{
    private readonly HttpClient _httpClient;
    private readonly StudentApiSettings _settings;

    public StudentCardVerificationService(
        HttpClient httpClient,
        IOptions<StudentApiSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", _settings.ApiKey);
    }
    
    public Task<Result<bool, Error>> VerifyStudentCardAsync(string studentCardId)
    {
        if (studentCardId.StartsWith("STUDENT_VALID"))
        {
            return Task.FromResult(Result.Success<bool, Error>(true));
        }
        else if (studentCardId.StartsWith("STUDENT_INVALID"))
        {
            return Task.FromResult(Result.Success<bool, Error>(false));
        }
        else
        {
            return Task.FromResult(Result.Failure<bool, Error>(Errors.General.ValueIsInvalid("Student card ID format")));
        }
        
        /*try
        {
            var response = _httpClient.GetAsync($"?cardId={studentCardId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<StudentApiResponse>(content);

                if (apiResponse?.IsValid == true)
                {
                    return Result.Success<bool, Error>(apiResponse.IsValid);
                }
                else
                {
                    return Result.Failure<bool, Error>(Errors.General.NotFound());
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result.Failure<bool, Error>(Errors.General.ExternalApiError(
                    $"Student API returned {response.StatusCode}: {errorContent}"));
            }
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<bool, Error>(
                Errors.General.ExternalApiError($"Network error connecting to student API: {ex.Message}"));
        }
        catch (JsonException ex)
        {
            return Result.Failure<bool, Error>(Errors.General.ExternalApiError($"JSON parsing error from student API: {ex.Message}"));
        }
        */
    }
}

public class StudentApiResponse
{
    public bool IsValid { get; set; }
    
    public string? Message { get; set; }
}