Imports System.IO

Public Class FormAddArticle


    Private Sub BtnReturnMenu_Click(sender As Object, e As RoutedEventArgs) Handles BtnReturnMenu.Click 'BOUTON RETOUR MENU

        Dim Accueil As New MainWindow

        Accueil.Show()
        Me.Hide()

    End Sub

    Public Class Data 'CLASSE DATA 

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

    '----------------------------------------------- AFFICHER LA LIGNE SELECTIONNEE DANS TEXTBOX -----------------------------------------------'

    Private Sub ListViewAddArticle_MouseClick(sender As Object, e As RoutedEventArgs) Handles ListViewAddArticle.MouseDoubleClick

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            Dim lines() As String = IO.File.ReadAllLines("C:\Users\lange\Documents\test.csv")

            If TextBoxCode.Text <> "" Or TextBoxCodeArt.Text <> "" Or TextBoxLibelle.Text <> "" Or TextBoxUnite.Text <> "" Then

                TextBoxCode.Clear()
                TextBoxCodeArt.Clear()
                TextBoxLibelle.Clear()
                TextBoxUnite.Clear()

            End If

            For i As Long = 0 To lines.Count - 1

                If lines(i) = lines(ListViewAddArticle.SelectedIndex + 1) Then

                    Dim splitComma As String() = lines(i).Split(",")

                    TextBoxCode.Text = splitComma(0)
                    TextBoxCodeArt.Text = splitComma(1)
                    TextBoxLibelle.Text = splitComma(2)
                    TextBoxUnite.Text = splitComma(3)

                End If

            Next

        End If

    End Sub

    '----------------------------------------------- ECRIRE LIGNE FICHIER CSV -----------------------------------------------'

    Private Sub BtnAddArticle_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddArticle.Click

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            Dim lines() As String = IO.File.ReadAllLines("C:\Users\lange\Documents\test.csv")

            For i As Long = 0 To lines.Count - 1

                Dim splitComma As String() = lines(i).Split(",")

                If splitComma(0) = TextBoxCode.Text Then ' Si le code existe déjà

                    MessageBox.Show("Ce code barre existe déjà.", "Erreur")
                    TextBoxCode.Clear()
                    Exit Sub

                End If
            Next

            If Char.IsDigit(TextBoxCode.Text) And TextBoxCode.Text.Length = 13 Then

                ListViewAddArticle.Items.Add(New Data(TextBoxCode.Text, TextBoxCodeArt.Text, TextBoxLibelle.Text, TextBoxUnite.Text, "0")) 'Remplit la liste

                Dim inputString As String = TextBoxCode.Text + "," + TextBoxCodeArt.Text + "," + TextBoxLibelle.Text + "," + TextBoxUnite.Text + "," + "0" + vbLf 'Stock dans la variable la string
                My.Computer.FileSystem.WriteAllText("C:\Users\lange\Documents\test.csv", inputString, True) 'Ecrit dans le fichier le contenu de la variable inputString

            Else

                MessageBox.Show("Un code doit être composé de 13 numéros.", "Erreur")
                TextBoxCode.Clear()
                Exit Sub

            End If

        End If

    End Sub


    '----------------------------------------------- MODIFIER LIGNE LISTE -----------------------------------------------'

    Private Sub BtnModifyArticle_Click(sender As Object, e As RoutedEventArgs) Handles BtnModifyArticle.Click

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            If (ListViewAddArticle.SelectedItems.Count > 0) Then

                Dim lines() As String = File.ReadAllLines("C:\Users\lange\Documents\test.csv")

                For i As Long = 0 To lines.Count - 1

                    If lines(i) = lines(ListViewAddArticle.SelectedIndex + 1) Then

                        Dim splitComma As String() = lines(i).Split(",")

                        If splitComma(0) = TextBoxCode.Text And splitComma(1) = TextBoxCodeArt.Text And splitComma(2) = TextBoxLibelle.Text And splitComma(3) = TextBoxUnite.Text Then 'Si aucune modifcation

                            MessageBox.Show("Rien à modifier.", "Erreur")
                            Exit Sub

                        ElseIf splitComma(0) = TextBoxCode.Text Then 'Si modification sauf code barre

                            lines(i) = splitComma(0) + "," + TextBoxCodeArt.Text + "," + TextBoxLibelle.Text + "," + TextBoxUnite.Text + "," + splitComma(4) 'Stock dans la variable la string

                            File.WriteAllLines("C:\Users\lange\Documents\test.csv", lines)
                            ListViewAddArticle.Items(ListViewAddArticle.SelectedIndex) = New Data(splitComma(0), TextBoxCodeArt.Text, TextBoxLibelle.Text, TextBoxUnite.Text, splitComma(4))

                        ElseIf splitComma(0) <> TextBoxCode.Text Then 'Si code barre modifié

                            MessageBox.Show("""Code barre"" ne peut pas être modifié.", "Erreur")
                            TextBoxCode.Text = splitComma(0)
                            TextBoxCodeArt.Text = splitComma(1)
                            TextBoxLibelle.Text = splitComma(2)
                            TextBoxUnite.Text = splitComma(3)
                            Exit Sub

                        End If

                    End If

                Next

            End If

        End If

    End Sub


    '----------------------------------------------- SUPPRIMER LIGNE LISTE -----------------------------------------------'

    Private Sub BtnDeleteArticle_Click(sender As Object, e As RoutedEventArgs) Handles BtnDeleteArticle.Click

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            If ListViewAddArticle.SelectedItems.Count > 0 Then

                Dim result = MessageBox.Show("Supprimer l'article ?", "Confirmer la suppression", MessageBoxButton.YesNo)

                If result = vbYes Then

                    Dim i As Integer = 0
                    Dim lines() As String = IO.File.ReadAllLines("C:\Users\lange\Documents\test.csv")

                    Using sw As StreamWriter = New StreamWriter("C:\Users\lange\Documents\test.csv")

                        For Each line In lines

                            If i <> ListViewAddArticle.SelectedIndex + 1 Then 'Faire + 1 car la listview commence à 0 (alors que le fichier commence à 0 une ligne avant)

                                sw.Write(line + vbLf) 'supprime la ligne sélectionnée et retour chariot dans le fichier

                            End If

                            i = i + 1

                        Next

                    End Using

                    ListViewAddArticle.Items.RemoveAt(ListViewAddArticle.SelectedIndex)

                ElseIf result = vbNo Then

                    Exit Sub

                End If

            Else

                MessageBox.Show("Aucun article sélécionné.", "Erreur")
                Exit Sub

            End If

        End If

    End Sub


    '----------------------------------------------- VOIR LISTE -----------------------------------------------'

    Private Sub BtnAdd_Click(sender As Object, e As RoutedEventArgs)

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            If ListViewAddArticle.Items IsNot Nothing Then 'Empeche d'afficher plusieurs la liste dans la ListView

                ListViewAddArticle.Items.Clear()

            End If

            Using MyReader As New FileIO.TextFieldParser("C:\Users\lange\Documents\test.csv")

                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")
                MyReader.ReadLine() 'Ignore le header

                Dim currentRow As String()

                While Not MyReader.EndOfData

                    Try
                        currentRow = MyReader.ReadFields()

                        ListViewAddArticle.Items.Add(New Data(currentRow(0), currentRow(1), currentRow(2), currentRow(3), currentRow(4)))


                    Catch ex As FileIO.MalformedLineException

                        MsgBox("Ligne " & ex.Message & "n'est pas valide et sera exclue.")

                    End Try

                End While

            End Using

        End If

    End Sub

End Class