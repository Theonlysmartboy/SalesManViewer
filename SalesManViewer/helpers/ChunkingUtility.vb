Namespace helpers
    Public Class ChunkingUtility
        Public Shared Function Chunk(Of T)(list As List(Of T), size As Integer) As List(Of List(Of T))
            Dim result As New List(Of List(Of T))

            For i = 0 To list.Count - 1 Step size
                result.Add(list.Skip(i).Take(size).ToList())
            Next

            Return result
        End Function
    End Class
End Namespace
