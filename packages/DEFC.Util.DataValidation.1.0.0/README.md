
## Objective

This is a cross platform **NET Standard 2.0 validation library** containing a bunch of validation code that I had to rewrite everytime I develop apps.
Due to the time I had to spend every day in validating data.I developed this library to help me and you save our development time.

## About

This library is one of **DEFC utilities** packages that contains several types of data validation methods, to help the developers minify their codes in easy way with no time.
Through this package can :

  * Check if a value is (Alphanumeric, Alpha, GUID, Base64, NullOrEmptyOrWhiteSpace, Email,URL,DateTime, Number, Byte, Short, Integer, Long, Decimal, Double, Float, IPv4, IPv6, IP, MACAddress, LatitudeLongitude )
  * Check if a value is between tow values, check if a value is between tow values or equal one of them, check if a value is equal to another value, check if value is greater than or equal another value and check if value is less than or equal another value.
  *  Match password and confirm password.
  * Check if password is strong with at least one uppercase, one lowercase, one digit  and one of the custom symbols with specific password length by sets the password rules.
  
Please feel free to use it.

## Example

Console application with full example available at [the GitHub repository](https://github.com/AminaElsheikh/DEFC.Util.DataValidationExamples).
```C#
using DEFC.Util.DataValidation;
using System.Text.RegularExpressions;
```
```C#
 public static void Validator()
        {           
            //Sample of data type validator
            bool IsValidAlphanumeric= DataType.IsAlphanumeric("Foo1234");
            bool IsValidGUID = DataType.IsGUID("am I a GUID");
            bool IsValidIPv4 = DataType.IsIPv4("127.0.0.1");
            bool IsValidURL = DataType.IsURL("https://www.nuget.org");
            //Sample of math validator
            bool IsValidNegative = Math.IsNegative(-1);
            bool IsValidEven = Math.IsEven(9);
            //Sample of comparison validator
            bool IsBetween = Comparison.IsBetween(4,2,10);
            bool IsLessThanOrEqual = Comparison.IsLessThanOrEqual(12,3);
            //Sample of SQL Injection validator
            bool HasSQLInjection = SQLInjection.IsExists("' or 1=1");
            //Sample of Regular Expression validator
            bool IsValidExpression = RegularExpression.IsMatch("Foo1234",new Regex("[a-zA-Z0-9]*$"));
            //Sample of Password validator
            bool Isvalid = Password.ValidatRules(new PasswordRules() 
                                                {
                                                 Password="Foo@123",
                                                 HasUpper=true,
                                                 HasLower=true,
                                                 HasDigit=true,
                                                 HasLength=true,
                                                 passwordMinLength=6,
                                                 HasSymbols=true,
                                                 symbols="@,&"
                                                });
        }
```
## Contributions

It will highly appreciated! Please make sure if works for ASP.NET and ASP.NET Core if possible and make sure it is covered by unit tests.

## License

MIS License
