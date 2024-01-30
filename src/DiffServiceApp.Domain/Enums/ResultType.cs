namespace DiffServiceApp.Domain.Enums;
public enum ResultType
{
    NotFound = 10,
    Equals = 20,
    SizeDoNotMatch = 30,
    ContentDoNotMatch = 40,
    ContentDoNotMatchButSizeIsEqual = 50
}
