Public Class DelegateCommand
    Implements ICommand

    Private _executeMethod As Action(Of Object)
    Private _canExecuteMethod As Func(Of Object, Boolean)

    Public Sub New(ByVal executeMethod As Action(Of Object), ByVal canExecuteMethod As Func(Of Object, Boolean))
        If executeMethod Is Nothing Then
            Throw New ArgumentNullException("executeMethod")
        End If
        _executeMethod = executeMethod
        _canExecuteMethod = canExecuteMethod
    End Sub

    Public Function CanExecute(ByVal parameter As Object) As Boolean Implements System.Windows.Input.ICommand.CanExecute
        If _canExecuteMethod Is Nothing Then
            Return True
        End If
        Return _canExecuteMethod(parameter)
    End Function

    Public Event CanExecuteChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements System.Windows.Input.ICommand.CanExecuteChanged

    Public Sub Execute(ByVal parameter As Object) Implements System.Windows.Input.ICommand.Execute
        _executeMethod(parameter)
    End Sub
End Class
