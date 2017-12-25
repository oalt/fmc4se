Public Class RequiredValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(value As Object, cultureInfo As Globalization.CultureInfo) As ValidationResult
        Dim result As ValidationResult = New ValidationResult(True, Nothing)
        If value Is Nothing OrElse value Is DBNull.Value Then
            result = New ValidationResult(False, "Required!")
        End If
        Return result
    End Function
End Class
