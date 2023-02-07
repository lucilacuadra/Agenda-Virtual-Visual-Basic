Imports System.ComponentModel
Imports System.IO
Public Class Form1
    Dim NomArchivo As String = "Datos.txt"
    Dim Ubicacion As String = "C:\GUI\GuardarDatos"
    Dim HayCambios As Boolean
    Dim nombre As String
    Dim direccion As String
    Dim telefono As String
    Dim Existe As Integer
    Dim Contar As Integer
    Dim Opc As Integer


    Private Sub btnLeer_Click_1(sender As Object, e As EventArgs) Handles BtnLeer.Click
        Dim Opc As Integer
        If HayCambios Then
            Opc = MsgBox("No se a guardado los ultimos cambios . ¿Desea guardarlos?", vbYesNo + vbCritical)
            If Opc = vbYes Then
                Me.Guardar(Ubicacion & NomArchivo)
            End If
        End If

        Me.Leer(Ubicacion & NomArchivo)

    End Sub


    Sub ControlarQueContengaCaracteresTexbox(nombre As String, direccion As String, telefono As String)
        If (nombre.ToString = "" OrElse direccion.ToString = "" OrElse telefono.ToString = "") Then
            MsgBox("¡Debe completar nombre, apellido y un teléfono!", vbCritical)
            Me.txtNombre.Focus()
        End If
        Exit Sub
    End Sub


    Sub Guardar(Archivo As String)
        Dim GrabarArchivo As New StreamWriter(Archivo)
        For i = 0 To lstDatos.Items.Count - 1
            Dim Linea = lstDatos.Items(i).ToString()
            GrabarArchivo.WriteLine(Linea)
        Next

        GrabarArchivo.Close()
        MsgBox("Archivo grabado Correctamente", vbInformation)
        HayCambios = False
        txtNombre.Focus()
    End Sub


    Sub Leer(Archivo As String)
        Dim leerArchivo As New StreamReader(Archivo)
        lstDatos.Items.Clear()
        Try
            While leerArchivo.Peek <> -1
                Dim linea As String = leerArchivo.ReadLine
                If String.IsNullOrEmpty(linea) Then
                    Continue While
                End If

                lstDatos.Items.Add(linea)
            End While

            leerArchivo.Close()
        Catch ex As Exception
            MsgBox("Se presento un problema al leer el archivo: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Sub Crear(Archivo As String)
        Dim CrearArchivo As FileStream = File.Create(Archivo)
        CrearArchivo.Close()
        Me.BtnLeer.Visible = True
        Me.BtnGuardar.Visible = True

    End Sub

    Private Sub lstDatos_SelectedIndexChanged_1(sender As Object, e As EventArgs)
        HayCambios = True
    End Sub


    Private Sub Form1_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.txtBuscador.Visible = False
        HayCambios = False
    End Sub

    Private Sub Form1_Closing_1(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If HayCambios Then
            If MsgBox("No se a guardado los ultimos cambios . ¿Desea guardarlos?", vbYesNo + vbCritical) = vbYes Then
                Me.Guardar(Ubicacion & NomArchivo)
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        If txtNombre.Text = "" Then
            MsgBox("Ingresar Nombre", vbCritical)
            txtNombre.Focus()
            Exit Sub
        ElseIf txtDireccion.Text = "" Then
            MsgBox("Ingresar Dirección", vbCritical)
            txtDireccion.Focus()
            Exit Sub
        ElseIf txtTelefono.Text = "" Then
            MsgBox("Ingresar Telefono", vbCritical)
            txtTelefono.Focus()
            Exit Sub
        End If
        nombre = Me.txtNombre.Text
        direccion = Me.txtDireccion.Text
        telefono = Me.txtTelefono.Text

        txtNombre.Text = Mid(txtNombre.Text, 1, 30)
        txtDireccion.Text = Mid(txtDireccion.Text, 1, 30)
        txtTelefono.Text = Mid(txtTelefono.Text, 1, 15)

        lstDatos.Items.Add("       " + nombre & Space(36 - Len(txtNombre.Text)) & direccion & Space(35 - Len(txtDireccion.Text)) & telefono & Space(20 - Len(txtTelefono.Text)))
        txtNombre.Text = ""
        txtDireccion.Text = ""
        txtTelefono.Text = ""

        ControlarQueContengaCaracteresTexbox(nombre, direccion, telefono)
        HayCambios = True
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click

        If lstDatos.SelectedIndex = -1 Then

            MsgBox("Seleccione la linea que desee editar", vbCritical)
            txtNombre.Focus()
            Exit Sub
        End If

        Dim nombre, direccion, telefono As String



        nombre = Mid(lstDatos.SelectedItem.ToString, 8, 35)
        direccion = Mid(lstDatos.SelectedItem.ToString, 44, 34)
        telefono = Mid(lstDatos.SelectedItem.ToString, 79, 13)

        Me.txtNombre.Text = nombre & Space(0 - Len(txtNombre.Text))
        Me.txtDireccion.Text = direccion & Space(0 - Len(txtDireccion.Text))
        Me.txtTelefono.Text = telefono & Space(0 - Len(txtTelefono.Text))

        lstDatos.Items.Remove(lstDatos.SelectedItem.ToString)
        HayCambios = True
        txtNombre.Focus()

    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        If Me.lstDatos.Items.ToString = "" Then
            MsgBox("Debe contener al menos un caracter", vbCritical,
                   "Ingrese Informacion")
            Exit Sub
        End If
        Me.Guardar(Ubicacion & NomArchivo)
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click

        If lstDatos.SelectedIndex = -1 Then
            MsgBox("Debe seleccionar un items para modificarlo", vbCritical)
            txtNombre.Focus()
            Exit Sub
        End If

        Dim opc As Integer
        opc = MsgBox("Desea eliminaro", vbYesNo)
        If opc = vbYes Then
            lstDatos.Items.Remove(lstDatos.SelectedItem.ToString())
        End If
        lstDatos.ClearSelected()
        txtNombre.Focus()

    End Sub

    Private Sub btnBuscador_Click(sender As Object, e As EventArgs) Handles btnBuscador.Click
        Static contadorClikcs As Integer
        contadorClikcs += 1
        If contadorClikcs = 1 Then
            Me.txtBuscador.Visible = True
        ElseIf contadorClikcs = 2 Then
            contadorClikcs = 0
            Me.txtBuscador.Visible = False
        End If
    End Sub

    Private Sub txtBuscador_TextChanged(sender As Object, e As EventArgs) Handles txtBuscador.TextChanged
        Dim i As Integer
        Dim existe As Integer
        If Len(Me.txtBuscador.Text) > 0 Then
            For i = 0 To lstDatos.Items.Count - 1
                existe = lstDatos.Items(i).indexOf(txtBuscador.Text)

                If existe <> -1 Then
                    Me.lstDatos.SelectedIndex() = i
                    Exit For
                End If
            Next i
        End If
    End Sub

    Private Sub txtTelefono_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTelefono.KeyPress
        If Not Char.IsNumber(e.KeyChar) And e.KeyChar <> Convert.ToChar(Keys.Back) Then
            MsgBox("Solo se permite numeros", vbInformation, "Atenicion")
            e.Handled = True
        End If
    End Sub
End Class

