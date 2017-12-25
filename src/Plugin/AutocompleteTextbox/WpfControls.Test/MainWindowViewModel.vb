Imports System.ComponentModel
Imports System.IO

Public Class MainWindowViewModel
    Implements INotifyPropertyChanged

#Region "Fields"

    Private _cancelCommand As ICommand
    Private _fileName As String
    Private _openCommand As ICommand
    Private _selectedPath As FileSystemInfo

#End Region 'Fields

#Region "Constructor"
    Public Sub New()
        SelectedPath = New DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
    End Sub
#End Region

#Region "Events"

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region 'Events

#Region "Properties"

    Public ReadOnly Property CancelCommand() As ICommand
        Get
            If _cancelCommand Is Nothing Then
                _cancelCommand = New DelegateCommand(AddressOf ExecuteCancelCommand, Nothing)
            End If
            Return _cancelCommand
        End Get
    End Property

    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property

    Public Property SelectedPath() As FileSystemInfo
        Get
            Return _selectedPath
        End Get
        Set(ByVal value As FileSystemInfo)
            _selectedPath = value
        End Set
    End Property

    Public ReadOnly Property OpenCommand() As ICommand
        Get
            If _openCommand Is Nothing Then
                _openCommand = New DelegateCommand(AddressOf ExecuteOpenCommand, Nothing)
            End If
            Return _openCommand
        End Get
    End Property

#End Region 'Properties

#Region "Methods"

    Protected Overridable Sub RaisePropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Private Sub ExecuteCancelCommand(ByVal param As Object)
        Application.Current.Shutdown()
    End Sub

    Private Sub ExecuteOpenCommand(ByVal param As Object)
        Try
            Process.Start(SelectedPath.FullName)
            Application.Current.Shutdown()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region 'Methods

End Class