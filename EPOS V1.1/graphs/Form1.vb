Imports System.Net
Imports System.IO
Imports System.Drawing.Drawing2D

Public Class Form1
    ' The ticker symbols.
    Private Symbols As List(Of String) = Nothing

    ' The current prices.
    Private Prices() As List(Of Single) = Nothing

    ' Redraw the graph.
    Private Sub picGraph_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles picGraph.Resize
        DrawGraph()
    End Sub

    ' Get the closing prices and graph them.
    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Me.Cursor = Cursors.WaitCursor

        ' Get the ticker symbols.
        Dim symbols_text() As String = txtSymbols.Text.Split(","c)
        Symbols = New List(Of String)()
        For i As Integer = 0 To symbols_text.Length - 1
            Symbols.Add(symbols_text(i).Trim())
        Next i

        ' Get the data.
        ReDim Prices(0 To Symbols.Count - 1)
        For i As Integer = 0 To Symbols.Count - 1
            Prices(i) = GetStockPrices(Symbols(i))
        Next i

        ' Graph it.
        DrawGraph()

        Me.Cursor = Cursors.Default
    End Sub

    ' Get the prices for this symbol.
    Private Function GetStockPrices(ByVal symbol As String) As List(Of Single)
        ' Compose the URL.
        Dim url As String = "http://www.google.com/finance/historical?output=csv&q=" & symbol

        ' Get the result.
        ' Get the web response.
        Dim result As String = GetWebResponse(url)

        ' Get the historical prices.
        Dim lines() As String = result.Split( _
            New String() {vbCr, vbLf}, _
            StringSplitOptions.RemoveEmptyEntries)
        Dim prices As New List(Of Single)()

        ' Process the lines, skipping the header.
        For i As Integer = 1 To lines.Length - 1
            Dim line As String = lines(i)
            prices.Add(Single.Parse(line.Split(","c)(4)))
        Next i

        Return prices
    End Function

    ' Get a web response.
    Private Function GetWebResponse(ByVal url As String) As String
        ' Make a WebClient.
        Dim web_client As New WebClient()

        ' Get the indicated URL.
        Dim response As Stream = web_client.OpenRead(url)

        ' Read the result.
        Using stream_reader As New StreamReader(response)
            ' Get the results.
            Dim result As String = stream_reader.ReadToEnd()

            ' Close the stream reader and its underlying stream.
            stream_reader.Close()

            ' Return the result.
            Return result
        End Using
    End Function

    ' Draw the graph.
    Private Sub DrawGraph()
        If (Prices Is Nothing) Then Return

        ' Make the bitmap.
        Dim bm As New Bitmap( _
            picGraph.ClientSize.Width, _
            picGraph.ClientSize.Height)
        Using gr As Graphics = Graphics.FromImage(bm)
            gr.Clear(Color.White)
            gr.SmoothingMode = SmoothingMode.AntiAlias

            ' Get the largest prices.
            Dim max_price As Single = 10
            For Each symbol_prices As List(Of Single) In Prices
                Dim new_max As Single = symbol_prices.Max()
                If (max_price < new_max) Then max_price = new_max
            Next symbol_prices

            ' Scale and translate the graph.
            Dim scale_x As Single = -picGraph.ClientSize.Width / CSng(Prices(0).Count)
            Dim scale_y As Single = -picGraph.ClientSize.Height / max_price
            gr.ScaleTransform(scale_x, scale_y)
            gr.TranslateTransform( _
                picGraph.ClientSize.Width, _
                picGraph.ClientSize.Height, _
                System.Drawing.Drawing2D.MatrixOrder.Append)

            ' Draw the grid lines.
            Using string_format As New StringFormat()
                Using thin_pen As New Pen(Color.Gray, 0)
                    For y As Integer = 0 To CInt(max_price) Step 10
                        gr.DrawLine(thin_pen, 0, y, Prices(0).Count, y)
                    Next y
                    For x As Integer = 0 To Prices(0).Count - 1 Step 7
                        gr.DrawLine(thin_pen, x, 0, x, 2)
                    Next x
                End Using
            End Using

            ' Draw each symbol's prices.
            Dim colors() As Color = {Color.Black, Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Purple}
            For symbol_num As Integer = 0 To Prices.Length - 1
                Dim symbol_prices As List(Of Single) = Prices(symbol_num)

                ' Make the data points.
                Dim points(0 To symbol_prices.Count - 1) As PointF
                For i As Integer = 0 To symbol_prices.Count - 1
                    points(i) = New PointF(i, symbol_prices(i))
                Next i

                ' Draw the points.
                Dim clr As Color = colors(symbol_num Mod colors.Length)
                Using thin_pen As New Pen(clr, 0)
                    gr.DrawLines(thin_pen, points)
                End Using

                ' Draw the symbol's name.
                DrawSymbolName(gr, Symbols(symbol_num), _
                    symbol_prices(symbol_prices.Count - 1), clr)
            Next symbol_num
        End Using

        ' Display the result.
        picGraph.Image = bm
    End Sub

    ' Draw the text at the specified location.
    Private Sub DrawSymbolName(ByVal gr As Graphics, ByVal txt As String, ByVal y As Single, ByVal clr As Color)
        ' See where the point is in PictureBox coordinates.
        Dim old_transformation As Matrix = gr.Transform
        Dim pt() As PointF = {New PointF(0, y)}
        gr.Transform.TransformPoints(pt)

        ' Reset the transformation.
        gr.ResetTransform()

        ' Draw the text.
        Using small_font As New Font("Arial", 8)
            Using br As New SolidBrush(clr)
                gr.DrawString(txt, small_font, br, 0, pt(0).Y)
            End Using
        End Using

        ' Restore the original transformation.
        gr.Transform = old_transformation
    End Sub
End Class
