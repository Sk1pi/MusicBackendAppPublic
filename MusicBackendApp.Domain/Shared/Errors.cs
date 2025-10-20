namespace MusicBackendApp.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value"; 
            return Error.Validation("value.is.invalid", $"{label} is invalid");
            
        }

        public static Error NotFound(string? id = null)
        {
            var forId = id == null ? "" : $" for id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var lablel = name == null ? "" : " " + name + " ";
            return Error.Validation("value.is.invalid", $"invalid{lablel}length");
        }
        
        public static Error ExternalApiError(string message) => 
            Error.Failure("external.api.error", message);
    }

    public static class AssignFamily
    {
        public static Error IsNotFamilySub(string s)
        {
            return Error.NotFound("User.IsNotFamilySub", "User not in family subscription");
        }
    }

    public static class Module
    {
        public static Error AlreadyExist(string s)
        {
            return Error.Validation("module.already exist", "module.already exist");
        }
    }

    public static class Payment
    {
        public static Error PaymentFailed(string s)
        {
            return Error.Validation("payment.failed", "payment failed");
        }
    }

    public static class Subscription
    {
        public static Error InvalidStudentCard()
        {
            return Error.Validation("subscription.invalid.studentcard", "subscription.invalid.studentcard");
        }
        public static Error InvalidSubscriptionType()
        {
            return Error.Validation("subscription.invalid.subscriptiontype", "subscription.invalid.subscriptiontype");
        }
        public static Error InvalidPaymentType()
        {
            return Error.Validation("subscription.invalid.paymenttype", "subscription.invalid.paymenttype");
        }
    }
    
    public static class Artist
    {
        public static Error NameAlreadyTaken(string name) =>
            new Error("Artist.NameAlreadyTaken", $"Artist name '{name}' is already taken.", ErrorType.Conflict);
    }

    public static class Users
    {
        public static Error TrackAlreadyInFavorites(string trackTitle) =>
            Error.Conflict("User.TrackAlreadyInFavorites", $"Track '{trackTitle}' is already in favorites.");
        
        public static Error TrackNotInFavorites() =>
            Error.NotFound("User.TrackNotInFavorites", "Track is not in favorites.");
    }
}