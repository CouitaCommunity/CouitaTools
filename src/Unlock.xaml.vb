Imports System.IO
Imports System.Net
Imports System.Threading.Tasks
Imports Microsoft.Win32

Public Class Unlock
    Private Sub Unlock_Click(sender As Object, e As RoutedEventArgs)
        Dim registryChanges As New List(Of Tuple(Of String, String, Object)) From {
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableSR", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableRestore", 0),
            New Tuple(Of String, String, Object)("HKCU:\Software\Policies\Microsoft\Windows", "DisableCMD", 0),
            New Tuple(Of String, String, Object)("HKCU:\Software\Policies\Microsoft\MMC", "RestrictToPermittedSnapins", 0),
            New Tuple(Of String, String, Object)("HKCU:\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableTaskMgr", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoFolderOptions", 0),
            New Tuple(Of String, String, Object)("HKCU:\Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop", "NoChangingWallPaper", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoWinKeys", 0),
            New Tuple(Of String, String, Object)("HKCU:\Software\Microsoft\Windows\CurrentVersion\Policies\System", "DisableChangePassword", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLockWorkstation", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "HidePowerOptions", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "DisableContextMenusInStart", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "StartMenuLogOff", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoClose", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "StartMenuPowerButton", 0),
            New Tuple(Of String, String, Object)("HKLM:\SYSTEM\CurrentControlSet\Control\Lsa", "DisableChangePassword", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastUserSwitching", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoControlPanel", 0),
            New Tuple(Of String, String, Object)("HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideIcons", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoCommonGroups", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoFind", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoLogoff", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoSetTaskbar", 0),
            New Tuple(Of String, String, Object)("HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableRegistryTools", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideFastSwitching", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Policies\Microsoft\Windows Defender", "DisableAntiSpyware", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "DisableBehaviorMonitoring", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "DisableOnAccessProtection", 0),
            New Tuple(Of String, String, Object)("HKLM\SOFTWARE\Policies\Microsoft\MicrosoftEdge\PhishingFilter", "EnabledV9", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "DisableScanOnRealtimeEnable", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "DisableIOAVProtection", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "DisableRealtimeMonitoring", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "LocalSettingOverrideDisableOnAccessProtection", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "LocalSettingOverrideRealtimeScanDirection", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "LocalSettingOverrideDisableIOAVProtection", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "LocalSettingOverrideDisableBehaviorMonitoring", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "LocalSettingOverrideDisableIntrusionPreventionSystem", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender\Real-Time Protection", "LocalSettingOverrideDisableRealtimeMonitoring", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Microsoft\Windows Defender", "PUAProtection", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Policies\Microsoft\Windows\System", "EnableSmartScreen", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Policies\Microsoft\MRT", "DontOfferThroughWUA", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Policies\Microsoft\MRT", "DontReportInfectionInformation", 0),
            New Tuple(Of String, String, Object)("HKLM:\SOFTWARE\Policies\Microsoft\Windows NT\SystemRestore", "DisableConfig", 0)
        }

        Dim progressStep As Double = 100.0 / registryChanges.Count
        ProgressBar1.Value = 0

        Task.Run(Sub()
                     For Each entry In registryChanges
                         Try
                             If entry.Item1 IsNot Nothing Then
                                 DeleteRegistryValue(entry.Item1, entry.Item2)
                             End If
                         Catch ex As Exception
                         Finally
                             ProgressBar1.Dispatcher.Invoke(Sub()
                                                                ProgressBar1.Value += progressStep
                                                            End Sub)
                         End Try
                     Next
                     ProgressBar1.Dispatcher.Invoke(Sub()
                                                        ProgressBar1.Value = 100
                                                    End Sub)
                 End Sub)
    End Sub

    Private Sub DeleteRegistryValue(key As String, valueName As String)
        Dim script As String = $"Remove-ItemProperty -Path '{key}' -Name '{valueName}' -ErrorAction SilentlyContinue"
        Try
            Using process As New Process()
                With process.StartInfo
                    .FileName = "powershell.exe"
                    .Arguments = "-Command " & script
                    .RedirectStandardOutput = True
                    .UseShellExecute = False
                    .CreateNoWindow = True
                End With
                process.Start()
                process.WaitForExit()
            End Using
        Catch ex As Exception
        End Try
    End Sub
End Class
