Imports System.IO

Public Class FormScanner

    Private Sub BtnReturnMenu_Click(sender As Object, e As RoutedEventArgs) Handles BtnReturnMenu.Click

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


    Public Sub New() 'Focus sur la textBox du Code pour recevoir le scan
        InitializeComponent()
        TextBoxCode.Focus()
    End Sub

    '----------------------------------------------- VALIDER SCAN -----------------------------------------------'

    Private Sub BtnValidate_Click(sender As Object, e As RoutedEventArgs) Handles BtnValidate.Click

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            Dim lines() As String = IO.File.ReadAllLines("C:\Users\lange\Documents\test.csv")

            For i As Long = 0 To lines.Count - 1

                If lines(i).Contains(TextBoxCode.Text) Then  ' Si l'article existe : Ajoute + 1 à sa quantité

                    Dim splitComma As String() = lines(i).Split(",")

                    If splitComma(0) = TextBoxCode.Text Then

                        splitComma(4) = splitComma(4) + 1

                        TextBoxCodeArt.Text = splitComma(1)
                        TextBoxLibelle.Text = splitComma(2)
                        TextBoxUnite.Text = splitComma(3)
                        TextBoxQuantity.Text = splitComma(4)

                        lines(i) = splitComma(0) + "," + splitComma(1) + "," + splitComma(2) + "," + splitComma(3) + "," + splitComma(4)

                        File.WriteAllLines("C:\Users\lange\Documents\test.csv", lines)

                    End If

                        Exit Sub


                ElseIf Not lines(i).Contains(TextBoxCode.Text) Then ' Si l'article n'existe pas : Créer l'article avec quantité = 1

                    Dim inputString As String = TextBoxCode.Text + "," + TextBoxCodeArt.Text + "," + TextBoxLibelle.Text + "," + TextBoxUnite.Text + "," + "1" + vbLf 'Stock dans la variable la string

                    If i = lines.Count - 1 Then

                        Dim result = MessageBox.Show("Créer l'article avec sa quantité ?", "Article inexistant", MessageBoxButton.YesNo)

                        If result = vbYes Then

                            If Char.IsDigit(TextBoxCode.Text) And TextBoxCode.Text.Length = 13 Then 'test le code avant écriture

                                My.Computer.FileSystem.WriteAllText("C:\Users\lange\Documents\test.csv", inputString, True) 'Ecrit dans le fichier le nouvel article avec sa quantité à 1

                            Else

                                MessageBox.Show("Un code doit être composé de 13 numéros.", "Erreur")
                                TextBoxCode.Clear()
                                Exit Sub

                            End If

                        ElseIf result = vbNo Then

                            Exit Sub

                        End If

                    End If

                End If

            Next

        End If

    End Sub

End Class
