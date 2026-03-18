Imports System.IO
Imports System.Reflection
Imports System.Text
Imports Microsoft.Web.WebView2.WinForms

Namespace helpers
    Public Class GoogleMapsHelper
        Private Const ICON_FOLDER As String = "marker_icons/"
        Private Const MAP_TEMPLATE As String = "SalesManViewer.googlemap_template.html"
        Private Const TEXT_TO_REPLACE_MARKER_DATA As String = "[[MARKER_DATA]]"
        Private Const TMP_NAME As String = "tmp_map.html"

        Private ReadOnly mWebView As WebView2
        Private ReadOnly mMarkerData As String(,)

        Public Sub New(wb As WebView2, markerData As String(,))
            mWebView = wb
            mMarkerData = markerData
        End Sub

        Public Async Function LoadMapAsync() As Task
            Await mWebView.EnsureCoreWebView2Async()
            Dim tmpHtmlPath = GetMapTemplate()
            If tmpHtmlPath IsNot Nothing Then
                mWebView.CoreWebView2.Navigate(tmpHtmlPath)
            End If
        End Function

        Private Function GetMapTemplate() As String
            Dim htmlTemplate As New StringBuilder(getStringFromResources(MAP_TEMPLATE))
            ' Build marker data string
            Dim mMarkerDataAsText As String = "["
            Dim rows As Integer = mMarkerData.GetLength(0)
            Dim cols As Integer = mMarkerData.GetLength(1)

            For i As Integer = 0 To rows - 1
                If i <> 0 Then mMarkerDataAsText += ","
                If cols = 2 Then
                    mMarkerDataAsText += $"[{mMarkerData(i, 0)},{mMarkerData(i, 1)}]"
                ElseIf cols = 3 Then
                    mMarkerDataAsText += $"[{mMarkerData(i, 0)},{mMarkerData(i, 1)},'{mMarkerData(i, 2)}']"
                ElseIf cols = 4 Then
                    Dim iconPath = Path.Combine(Path.GetDirectoryName(GetType(GoogleMapsHelper).Assembly.Location), ICON_FOLDER, mMarkerData(i, 3)).Replace("\", "/")
                    mMarkerDataAsText += $"[{mMarkerData(i, 0)},{mMarkerData(i, 1)},'{mMarkerData(i, 2)}','{iconPath}']"
                End If
            Next
            mMarkerDataAsText += "]"
            htmlTemplate.Replace(TEXT_TO_REPLACE_MARKER_DATA, mMarkerDataAsText)
            ' Write temp file
            Dim tmpHtmlMapFile As String = Path.Combine(Path.GetTempPath(), TMP_NAME)
            File.WriteAllText(tmpHtmlMapFile, htmlTemplate.ToString())
            Return tmpHtmlMapFile
        End Function

        Private Function getStringFromResources(resourceName As String) As String
            Dim asm As Assembly = GetType(GoogleMapsHelper).Assembly
            Using stream As Stream = asm.GetManifestResourceStream(resourceName)
                Using reader As New StreamReader(stream)
                    Return reader.ReadToEnd()
                End Using
            End Using
        End Function
    End Class
End Namespace