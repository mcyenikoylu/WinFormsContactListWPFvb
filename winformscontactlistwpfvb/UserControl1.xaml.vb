Imports System.Data.SQLite
Imports System.Data
Imports System.IO
Imports System.Windows.Media.Imaging
Imports System.Windows.Data

Public Class UserControl1
    Private Sub UserControl_Loaded(sender As Object, e As Windows.RoutedEventArgs)
        Data("")
    End Sub
    Private Sub Data(ByVal searchtext As String)
        Dim connstr As SQLiteConnection = New SQLiteConnection("Data Source=xpress_entry_sample.db3;Version=3")
        connstr.Open()
        Dim cmd As SQLiteCommand = New SQLiteCommand()
        cmd.Connection = connstr

        If searchtext.Trim() <> "" Then
            cmd.CommandText = "select u.first_name, u.last_name, u.id_image, b.badge_no, u.login, p.picture, u.email from users u" & " left outer join badges b on u.id = b.user_id" & " left outer join pictures p on p.id = u.picture_id" & " where u.first_name like '%" & searchtext & "%' or u.last_name like '%" & searchtext & "%' or b.badge_no like '%" & searchtext & "%' or u.login like '%" & searchtext & "%' or p.picture like '%" & searchtext & "%' or u.email like '%" & searchtext & "%'    "
        Else
            cmd.CommandText = "select u.first_name, u.last_name, u.id_image, b.badge_no, u.login, p.picture, u.email from users u" & " left outer join badges b on u.id = b.user_id" & " left outer join pictures p on p.id = u.picture_id"
        End If

        Dim rdr As SQLiteDataReader = cmd.ExecuteReader()
        Dim dt As DataTable = New DataTable()
        dt.Load(rdr)
        rdr.Close()
        connstr.Close()
        myListBox.ItemsSource = dt.DefaultView
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As Windows.Controls.TextChangedEventArgs)
        Dim text As String = e.Source.ToString()
        Dim strSplite As String() = text.Split(" "c)

        If strSplite(0).ToString() = "System.Windows.Controls.TextBox:" Then
            Data(strSplite(1).ToString())
        ElseIf strSplite(0).ToString() = "System.Windows.Controls.TextBox" Then
            Data("")
        End If
    End Sub
End Class
Public Class BinaryImageConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object,
                            ByVal targetType As Type,
                            ByVal parameter As Object,
                            ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        'If (TypeOf ((Not (value) Is Nothing) _
        '            AndAlso value) Is Byte()) Then
        If value IsNot Nothing AndAlso TypeOf value Is Byte() Then

            Dim ByteArray() As Byte = CType(value, Byte())
            Dim bmp As BitmapImage = New BitmapImage
            bmp.BeginInit()
            bmp.StreamSource = New MemoryStream(ByteArray)
            bmp.EndInit()
            Return bmp
        End If

        Return Nothing
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New Exception("The method or operation is not implemented.")
    End Function
End Class