Public Class Form1
    Private Sub AddStep_Click(sender As Object, e As EventArgs) Handles AddStep.Click
        Dim temp As String
        Dim time As String
        Dim total As String
        temp = TemperatuurTextBox.Text
        time = TijdTextBox.Text
        If Int(temp) > 100 Then
            MsgBox("Temperature outside of acceptable range!")
        ElseIf Int(time) < 0 Then
            MsgBox("Time outside of acceptable range!")
        Else
            total = temp & " C - " & time & " min"
            ListBox1.Items.Add(total)
            TemperatuurTextBox.Text = ""
            TijdTextBox.Text = ""
        End If
    End Sub

    Private Sub StepDel_Click(sender As Object, e As EventArgs) Handles StepDel.Click
        Dim toDel As Integer
        toDel = ListBox1.SelectedIndex
        ListBox1.Items.RemoveAt(toDel)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ListBox1.SelectedIndex > 0 Then
            Dim I = ListBox1.SelectedIndex - 1
            ListBox1.Items.Insert(I, ListBox1.SelectedItem)
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
            ListBox1.SelectedIndex = I
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.SelectedIndex < ListBox1.Items.Count - 1 Then
            Dim I = ListBox1.SelectedIndex + 2
            ListBox1.Items.Insert(I, ListBox1.SelectedItem)
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
            ListBox1.SelectedIndex = I - 1
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If SerialPort1.IsOpen Then
            SerialPort1.Write(TextBox1.Text)

        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Not SerialPort1.IsOpen Then
            SerialPort1.Open()
            Timer1.Enabled = True
        End If

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If SerialPort1.IsOpen Then
            Timer1.Enabled = False
            SerialPort1.Close()
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        'stop the timer (stops this function being called while it is still working
        Timer1.Enabled = False

        Dim incoming As String
        ' get any new data and add the the global variable receivedData
        incoming = RequestSerialData()
        Dim msgsplit As Array
        msgsplit = Split(incoming, ".")
        'If receivedData contains a "<" and a ">" then we have data
        RichTextBox1.Text += incoming
        RichTextBox1.Text += "\n"

        For Each item As String In msgsplit
            If Not item.Equals("") Then
                Dim dId As String
                dId = item.Substring(0, 1)
                Dim data As Double
                data = CDbl(item.Replace(dId, ""))
                If dId.Equals("t") Then
                    Chart1.Series("Temperatuur").Points.AddXY(DateTime.Now.ToString("H:mm:ss"), CDbl(data))
                    If Chart1.Series(0).Points.Count > 50 Then
                        Chart1.ChartAreas(0).AxisX.ScaleView.Position = Chart1.Series(0).Points.Count - 50
                        Chart1.ChartAreas(0).AxisX.ScaleView.Size = 50
                    End If
                ElseIf dId.Equals("h") Then
                    If (data > 0) Then
                        Label9.BackColor = Drawing.Color.Green
                    Else
                        Label9.BackColor = Drawing.Color.Red
                    End If
                ElseIf dId.Equals("H") Then
                    If data > 0 Then
                        Label10.BackColor = Drawing.Color.Green
                    Else
                        Label10.BackColor = Drawing.Color.Red
                    End If
                End If
            End If
        Next

        ' restart the timer
        Timer1.Enabled = True
    End Sub

    Function RequestSerialData() As String
        ' returns new data from the serial connection
        SerialPort1.Write("rfd")
        Dim Incoming As String
        Try
            Incoming = SerialPort1.ReadLine()
            Return Incoming.Replace(vbCr, "")
        Catch ex As TimeoutException
            Return "Error: Serial Port read timed out."
        End Try

    End Function


    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        For Each sp As String In System.IO.Ports.SerialPort.GetPortNames()
            ListBox2.Items.Add(sp)
        Next
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim Selected As String
        Selected = ListBox2.SelectedItem
        SerialPort1.PortName = Selected
        Label8.Text = SerialPort1.PortName
    End Sub


End Class