Public Class FilesystemSuggestionProvider
    Implements ISuggestionProvider

    Public Function GetSuggestions(ByVal filter As String) As System.Collections.IEnumerable Implements ISuggestionProvider.GetSuggestions
        If String.IsNullOrEmpty(filter) Then
            Return Nothing
        End If
        If filter.Length < 3 Then
            Return Nothing
        End If

        If filter(1) <> ":"c Then
            Return Nothing
        End If

        Dim lst As New List(Of IO.FileSystemInfo)
        Dim dirFilter As String = "*"
        Dim dirPath As String = filter
        If Not filter.EndsWith("\") Then
            Dim index As Integer = filter.LastIndexOf("\")
            dirPath = filter.Substring(0, index + 1)
            dirFilter = filter.Substring(index + 1) + "*"
        End If
        Dim dirInfo As IO.DirectoryInfo = New IO.DirectoryInfo(dirPath)
        lst.AddRange(dirInfo.GetFileSystemInfos(dirFilter))
        System.Threading.Thread.Sleep(2000)
        Return lst
    End Function

End Class
