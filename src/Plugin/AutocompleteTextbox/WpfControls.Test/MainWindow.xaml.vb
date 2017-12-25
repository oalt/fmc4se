Class MainWindow 

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        DataContext = New MainWindowViewModel
    End Sub
End Class
