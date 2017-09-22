Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim daba As New Database
        LabelCon.Text = $"Conectado al servidor {daba.Server}"
    End Sub

    Private Sub textbox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        Dim codigo As String = TextBox1.Text
        'variables locales
        Dim rac As String
        Dim tipo, peso, piezas As Integer
        Dim guia As Long
        Dim origen As String = ""

        If e.KeyCode = Keys.Enter Then
            ' cuando se presiona enter
            'MsgBox("Puyaste enter")
            Try
                rac = Strings.Left(codigo, 2)
                'agarro los 2 primeros caracteres de una
                If EsUbicacion(rac) Then
                    ' es una ubicacion y no guia
                    If rac = "AF" Then
                        rac = "AFUERA"
                    End If
                    'TextBox14.Text = rac

                    'Sino es ubicacion hago lo propio
                ElseIf Len(codigo) < 8 Then
                    'guia normal
                    tipo = 0
                    guia = codigo

                ElseIf Len(codigo) = 30 Then
                    ' Codigo de barra completo
                    tipo = rac
                    guia = Mid(codigo, 8, 8)
                    origen = Mid(codigo, 3, 5)
                    'MsgBox("LA GUIA ES " & guia)
                    peso = Strings.Right(codigo, 6)
                    'TextBox11.Text = peso
                    piezas = Mid(codigo, 22, 3)
                    'TextBox12.Text = piezas
                ElseIf Len(codigo) = 15 Then
                    ' guia factura
                    tipo = rac
                    origen = Mid(codigo, 3, 5)
                    guia = Mid(codigo, 8, 8)

                End If
            Catch es As Exception
                MsgBox("Debe introducir algun valor", MsgBoxStyle.Information, "Lo siento")
            End Try
            'TextBox1.Text = guia

            'INtroducir en la base de datos
            DataGridView1.Rows.Add({guia, origen, tipo, piezas, peso, CheckBox1.Checked})
            TextBox1.Clear()
            TextBox1.Select()
            LabelInfo.Text = "Total pistoleado: " & DataGridView1.Rows.Count
        End If
    End Sub

    Public Function EsUbicacion(ByVal leer As String) As Boolean

        If leer = "1A" Or leer = "1B" Or leer = "1D" Or leer = "1C" Or leer = "1D" _
         Or leer = "1E" Or leer = "1F" Or leer = "1G" Or leer = "1H" _
          Or leer = "2A" Or leer = "2B" Or leer = "2C" Or leer = "2D" Or leer = "AF" Then

            EsUbicacion = True
        Else
            EsUbicacion = False
        End If

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim guia As Long
        Dim origen As String
        Dim tipo, piezas, peso As Integer
        Dim reparto As Boolean

        Try
            guia = TextBox1.Text
            origen = TextBox2.Text
            tipo = TextBox3.Text
            piezas = TextBox4.Text
            peso = TextBox5.Text
            reparto = CheckBox1.Checked
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            DataGridView1.Rows.Add({guia, origen, tipo, piezas, peso, CheckBox1.Checked})
            TextBox1.Clear()
            TextBox2.Clear() : TextBox3.Clear() : TextBox4.Clear() : TextBox5.Clear()
            TextBox1.Select()
            LabelInfo.Text = "Total pistoleado: " & DataGridView1.Rows.Count
        End Try


    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles BtnAgregar.Click
        Dim re, inser, insertadas, actualizadas As Integer
        For Each fila As DataGridViewRow In DataGridView1.Rows
            Dim daba As New Database
            re = daba.ActualizarDGVRow(fila.Cells(0).Value, fila.Cells("origen").Value, fila.Cells("tipo").Value,
                                fila.Cells("peso").Value, fila.Cells("piezas").Value, fila.Cells("reparto").Value)
            If re < 1 Then
                ' No esta esa guia
                inser = daba.InsertarDGVRow(fila.Cells(0).Value, fila.Cells("origen").Value, fila.Cells("tipo").Value,
                                fila.Cells("peso").Value, fila.Cells("piezas").Value, fila.Cells("reparto").Value)
                insertadas += inser
            Else
                actualizadas += re
            End If
            'MessageBox.Show("se ha inertado " & re)
            LabelResult.Text = $"se verifican {actualizadas} paquetes, donde se han agregado {insertadas} paquetes que no estaban"
        Next
        Dim dab As New Database
        LabelResult.Text &= ". " & dab.UbicarReparto() & "paq van a reparto"
        'borrar toa mierda
        DataGridView1.Rows.Clear()

    End Sub
End Class
