Imports MySql.Data.MySqlClient

Public Class Database

    Private conexion As MySqlConnection

    Private _server As String
    Public Property Server() As String
        Get
            Return _server
        End Get
        Set(ByVal value As String)
            _server = value
        End Set
    End Property

    Public Property user As String
    Public Property Pass As String
    Private _db As String
    Public Property Db() As String
        Get
            Return _db
        End Get
        Set(ByVal value As String)
            _db = value
        End Set
    End Property
    ' Constructor Method
    Public Sub New()
        Try            Db = "envios"
            Server = "192.168.0.111"
            user = "vit"
            Pass = ""
            FileOpen(1, "conexion.txt", OpenMode.Input)
            Input(1, Server)
            Input(1, Db)
            Input(1, user)
            Input(1, Pass)
            FileClose(1)

            conexion = New MySqlConnection()
            conexion.ConnectionString = "server=" & Server & ";" &                "user id=" & user & ";" &                "password=" & Pass & ";" &                "port=" & 3306 & ";" &                 "database=" & Db & ";"
            conexion.Open()
        Catch ex As Exception
            MsgBox("Error al conectar al servidor MySQL " & vbCrLf & vbCrLf & ex.Message & conexion.State, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
        End Try
    End Sub
    Public Function Agregar(guia, nombre, rac, peso, piezas, precio, tel)        Dim sql As New MySqlCommand

        sql.CommandText = $"INSERT INTO inventario(guia, nombre, peso,piezas,precio,rac,tel,fecha) VALUES({guia}, '{nombre}', {peso} , {piezas}, '{precio}','{rac}','{tel}','{Now}' ); SELECT LAST_INSERT_ID()"
        Return Sql.ExecuteScalar()
    End Function
    Public Function InsertarDGVRow(guia As Long, origen As String, tipo As Integer, peso As Integer, piezas As Integer, reparto As Boolean) As Integer
        Dim sql As New MySqlCommand
        sql.Connection = conexion
        sql.CommandText = $"INSERT INTO inventario(guia, origen,tipo,peso,piezas,reparto,fecha) VALUES({guia}, '{origen}',{tipo},{peso},{piezas},{reparto},'{Now.ToString}')"        'conexion.Open()
        Return sql.ExecuteNonQuery()
    End Function
    Public Function ActualizarDGVRow(guia As Long, origen As String, tipo As Integer, peso As Integer, piezas As Integer, reparto As Boolean) As Integer
        Dim sql As New MySqlCommand
        sql.Connection = conexion
        sql.CommandText = $"UPDATE inventario SET origen='{origen}', peso={peso},piezas={piezas},reparto={reparto} WHERE guia={guia}"        'conexion.Open()
        Return sql.ExecuteNonQuery()
    End Function

    Public Function UbicarReparto() As Integer
        Dim sql As New MySqlCommand
        sql.Connection = conexion
        sql.CommandText = "UPDATE inventario SET rac='REPARTO' WHERE reparto=1 and entregado=0"        'conexion.Open()
        Return sql.ExecuteNonQuery()
    End Function
End Class

