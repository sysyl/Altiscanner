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

    '----------------------------------------------- GESTION INTERFACE CODE -----------------------------------------------'

    Public Sub New() 'Focus sur la textBox du Code

        InitializeComponent()
        TextBoxCode.Focus()

    End Sub

    Private Sub TextBoxCode_KeyDown(sender As Object, e As KeyboardEventArgs) Handles TextBoxCode.KeyDown 'Annalyse le code avant validation

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MessageBox.Show("Scan impossible : Fichier CSV inexistant.", "Erreur")

        Else

            If TextBoxCode.Text.Length = 13 Then 'Test longueur -> appuyer sur une touche du clavier après insertion du code

                Dim lines() As String = IO.File.ReadAllLines("C:\Users\lange\Documents\test.csv")

                For i As Long = 0 To lines.Count - 1

                    If lines(i).Contains(TextBoxCode.Text) Then  ' Si le code barre existe : Afficher l'article dans les textBox

                        Dim splitComma As String() = lines(i).Split(",")

                        If splitComma(0) = TextBoxCode.Text Then

                            TextBoxCodeArt.Text = splitComma(1)
                            TextBoxLibelle.Text = splitComma(2)
                            TextBoxUnite.Text = splitComma(3)
                            TextBoxQuantity.Text = splitComma(4)

                            TextBoxCodeArt.IsEnabled = False
                            TextBoxLibelle.IsEnabled = False
                            TextBoxUnite.IsEnabled = False

                        End If

                        Exit Sub

                    ElseIf Not lines(i).Contains(TextBoxCode.Text) Then ' Si le code barre n'existe pas

                        If i = lines.Count - 1 Then

                            For Each Ch As Char In TextBoxCode.Text

                                If Not Char.IsNumber(Ch) Then

                                    MessageBox.Show("Le code barre doit contenir seulement des chiffres.", "Erreur")
                                    TextBoxCode.Clear()

                                    Exit Sub

                                End If

                            Next

                            MessageBox.Show("Ce code barre n'existe pas.", "Information")

                            TextBoxCodeArt.Clear()
                            TextBoxLibelle.Clear()
                            TextBoxUnite.Clear()
                            TextBoxQuantity.Clear()

                            TextBoxCodeArt.IsEnabled = True
                            TextBoxLibelle.IsEnabled = True
                            TextBoxUnite.IsEnabled = True

                        End If

                    End If

                Next

            End If

        End If

    End Sub


    '----------------------------------------------- ECRITURE : VALIDER SCAN -----------------------------------------------'

    Private Sub BtnValidate_Click(sender As Object, e As RoutedEventArgs) Handles BtnValidate.Click

        If File.Exists("C:\Users\lange\Documents\test.csv") = False Then

            MsgBox("Fichier inexistant.")

        Else

            Dim lines() As String = IO.File.ReadAllLines("C:\Users\lange\Documents\test.csv")

            For i As Long = 0 To lines.Count - 1

                If lines(i).Contains(TextBoxCode.Text) Then  ' Si le code barre existe : Ajoute + 1 à la quantité

                    Dim splitComma As String() = lines(i).Split(",")

                    If splitComma(0) = TextBoxCode.Text Then

                        splitComma(4) = splitComma(4) + 1
                        TextBoxQuantity.Text = splitComma(4)
                        lines(i) = splitComma(0) + "," + splitComma(1) + "," + splitComma(2) + "," + splitComma(3) + "," + splitComma(4)
                        File.WriteAllLines("C:\Users\lange\Documents\test.csv", lines)

                    End If

                    Exit Sub


                ElseIf Not lines(i).Contains(TextBoxCode.Text) Then ' Si le code barre n'existe pas : Créer l'article avec quantité = 1

                    Dim inputString As String = TextBoxCode.Text + "," + TextBoxCodeArt.Text + "," + TextBoxLibelle.Text + "," + TextBoxUnite.Text + "," + "1" + vbLf 'Stock dans la variable la string

                    If i = lines.Count - 1 Then

                        Dim result = MessageBox.Show("Créer l'article avec sa quantité ?", "Article inexistant", MessageBoxButton.YesNo)

                        If result = vbYes Then

                            If TextBoxCodeArt.Text = "" Or TextBoxLibelle.Text = "" Or TextBoxUnite.Text = "" Then 'test le code avant écriture

                                MessageBox.Show("Une ou plusieurs cases sont vides.", "Erreur")
                                Exit Sub

                            ElseIf TextBoxCode.Text.Length <> 13 Then

                                MessageBox.Show("Longueur du code barre non valide.", "Erreur")
                                Exit Sub

                            Else 'Test si le code est numérique

                                Dim bool As Boolean = True

                                For Each Ch As Char In TextBoxCode.Text

                                        If Not Char.IsNumber(Ch) Then

                                            bool = False

                                        End If

                                    Next

                                If bool = False Then

                                    MessageBox.Show("Le code barre doit contenir seulement des chiffres.", "Erreur")
                                    TextBoxCode.Clear()

                                Else 'Ecriture

                                    TextBoxQuantity.Text = "1"

                                    TextBoxCodeArt.IsEnabled = False
                                    TextBoxLibelle.IsEnabled = False
                                    TextBoxUnite.IsEnabled = False

                                    My.Computer.FileSystem.WriteAllText("C:\Users\lange\Documents\test.csv", inputString, True) 'Ecrit dans le fichier le nouvel article avec sa quantité à 1

                                End If

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
