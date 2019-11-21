Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class MenuScene
    Inherits GameScene

    Private naval As Texture2D
    Private font As SpriteFont

    Private texts As String()
    Private game As Game

    Private contextSizeMap As GUIContext

    Private numberSelectorX As NumberSelector
    Private numberSelectorY As NumberSelector


    Public Sub New(ByRef game As Game)
        updates = New LinkedList(Of IUpdate)
        Me.game = game
    End Sub

    Private Function CreateTexts() As String()
        Dim texts As String() = New String() {"Play", "Exit"}
        Return texts
    End Function

    Private Sub StartButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        GUIController.ChangeContext(contextSizeMap)
    End Sub

    Private Sub OkButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        ScreenManager.Instance.ChangeScene(New NavalBattleScene(numberSelectorX.Value, numberSelectorY.Value))
    End Sub

    Private Sub ExitButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        game.Exit()
    End Sub

    Private Function CreateContextSizeMap() As GUIContext
        Dim context As GUIContext = New GUIContext(Camera.InternalDimensions)

        numberSelectorX = New NumberSelector(0, 0, 0, Vector2.Zero, 6, 100)
        numberSelectorY = New NumberSelector(1, 0, 1, Vector2.Zero, 6, 100)
        Dim button As Button = New Button(2, 0, 2, Vector2.Zero, "Ok", Vector2.One * 0.6)

        numberSelectorX.Position = New Vector2(0, -numberSelectorX.GetSize().Y * 4.0F / 3.0F)
        numberSelectorY.Position = New Vector2(0, +numberSelectorY.GetSize().Y * 4.0F / 3.0F)
        button.Position = New Vector2(0, numberSelectorY.Position.Y + numberSelectorY.GetSize().Y * 3.0F / 2.0F)

        button.OnFire0 = AddressOf OkButton

        context.Add(numberSelectorX)
        context.Add(numberSelectorY)
        context.Add(button)

        Return context
    End Function

    Private Sub CreateMenu()
        Dim horizontalFrame As Frame = New Frame(naval, New Rectangle(80, 120, 8, 8))

        Dim selectFrame As Frame = New Frame(naval, New Vector2(0, 0), New Rectangle(88, 96, 8, 8), Color.White, SpriteEffects.None)

        Dim i As Integer = 0
        For Each text As String In texts
            Dim button As Button = New Button(i, 0, i, New Vector2(0, (i + 2 - texts.Count) * 24.0F), text, Vector2.One * 0.5)
            GUIController.MainContext.Add(button)
            Select Case i
                Case 0
                    button.OnFire0 = AddressOf StartButton
                Case 1
                    button.OnFire0 = AddressOf ExitButton
            End Select
            i += 1
        Next
    End Sub


    Public Overrides Sub LoadContent()
        MyBase.LoadContent()
        naval = content.Load(Of Texture2D)("naval")
        font = content.Load(Of SpriteFont)("fonts/PressStart2P")

        texts = CreateTexts()

        CreateMenu()

        contextSizeMap = CreateContextSizeMap()
    End Sub

End Class
