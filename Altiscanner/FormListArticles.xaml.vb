Imports System.IO

Public Class FormListArticles


    Private Sub BtnReturnMenu_Click(sender As Object, e As RoutedEventArgs) Handles BtnReturnMenu.Click 'BOUTON RETOUR MENU

        Dim Accueil As New MainWindow
        Accueil.Show()
        Me.Hide()

    End Sub

    Private Class Data 'CLASSE DATA

        Public Sub New(col1 As String, col2 As String, col3 As String, col4 As String, col5 As String)

            Me.CodeBarre = col1
            Me.CodeArt = col2
            Me.Libelle = col3
            Me.Unite = col4
            Me.Quantite = col5

        End Sub

        Public Property CodeBarre As String
        Public Property CodeArt As String
        Public Property Libelle As String
        Public Property Unite As String
        Public Property Quantite As String

    End Class

    '----------------------------------------------- RECHERCHE ARTICLE -----------------------------------------------'

    Private Sub Recherche_TextChanged(sender As Object, e As RoutedEventArgs) Handles TextBoxRecherche.TextChanged

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            If monListViewRecherche.Items IsNot Nothing Then 'Empeche d'afficher plusieurs fois la recherche dans la ListView

                monListViewRecherche.Items.Clear()

            End If

            Dim lines() As String = File.ReadAllLines("C:\Users\lange\Documents\test.csv")

            For i As Long = 0 To lines.Count - 1

                Dim splitComma As String() = lines(i).Split(",")

                If lines(i).Contains(TextBoxRecherche.Text) And TextBoxRecherche.Text <> "" Then

                    monListViewRecherche.Items.Add(New Data(splitComma(0), splitComma(1), splitComma(2), splitComma(3), splitComma(4)))

                End If

            Next

        End If

    End Sub


    '----------------------------------------------- VOIR LISTE -----------------------------------------------'

    Private Sub BtnAdd_Click(sender As Object, e As RoutedEventArgs)

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            If monListView.Items IsNot Nothing Then 'Empeche d'afficher plusieurs fois la liste dans la ListView

                monListView.Items.Clear()

            End If

            Using MyReader As New FileIO.TextFieldParser("C:\Users\lange\Documents\test.csv")

                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")
                MyReader.ReadLine() 'Ignore le header

                Dim currentRow As String()

                While Not MyReader.EndOfData

                    Try
                        currentRow = MyReader.ReadFields()

                        monListView.Items.Add(New Data(currentRow(0), currentRow(1), currentRow(2), currentRow(3), currentRow(4)))


                    Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException

                        MsgBox("Ligne " & ex.Message & "n'est pas valide et sera exclue.")

                    End Try

                End While

            End Using

        End If

    End Sub

End Class
