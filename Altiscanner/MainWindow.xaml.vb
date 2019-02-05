Class MainWindow

    Private Sub BtnScanner_Click(sender As Object, e As RoutedEventArgs) Handles BtnScanner.Click
        Dim FormScanner As New FormScanner
        FormScanner.Show()
        Me.Hide()
    End Sub

    Private Sub BtnAddArticle_Click(sender As Object, e As RoutedEventArgs) Handles BtnAddArticle.Click
        Dim FormAddArticle As New FormAddArticle
        FormAddArticle.Show()
        Me.Hide()
    End Sub

    Private Sub BtnListArticle_Click(sender As Object, e As RoutedEventArgs) Handles BtnListArticle.Click
        Dim FormListArticles As New FormListArticles
        FormListArticles.Show()
        Me.Hide()
    End Sub

End Class