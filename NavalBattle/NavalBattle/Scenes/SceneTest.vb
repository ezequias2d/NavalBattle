Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class SceneTest
    Inherits GameScene

    Private naval As Texture2D
    Private font As SpriteFont

    Private texts As String()
    Private game As Game

    Public Sub New(ByRef game As Game)
        updates = New LinkedList(Of IUpdate)
        Me.game = game
    End Sub

    Private Function CreateTexts() As String()
        Dim texts As String() = New String() {"Play", "Exit"}
        Return texts
    End Function

    Private Sub StartButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        ScreenManager.Instance.ChangeScene(New NavalBattleScene(15, 10))
    End Sub

    Private Sub ExitButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        game.Exit()
    End Sub

    Private Function CreateContext(div As Integer) As GUIContext
        Dim frame As Frame = GUIController.CreateSimpleFrame()
        Dim context As GUIContext = New GUIContext(New Vector2(48, 24), frame, frame, New Frame(GUIController.Texture, New Rectangle(96, 0, 16, 16)), New Frame(GUIController.Texture, New Rectangle(112, 0, 16, 16)), New Frame(naval, New Rectangle(96, 96, 24, 24)))
        context.Add(New Button(0, 0, 0, New Vector2(-10, -10), "Botao1", Vector2.One * 0.6))
        context.Add(New NumberSelector(0, 0, 1, New Vector2(20, 20), 6, 30))
        Return context
    End Function

    Private Sub CreateGUIContext(context As GUIContext)
        Dim selectFrame As Frame = New Frame(naval, New Vector2(0, 0), New Rectangle(88, 96, 8, 8), Color.White, SpriteEffects.None)
        Dim i As Integer = 0
        For Each text As String In texts
            'Dim button As SimpleButton = New SimpleButton(i, 0, i, New Vector2(0, (i + 2 - texts.Count) * 24.0F), selectFrame, New Label(text, Color.White, font))
            Dim button As GUIContext = CreateContext(texts.Count)
            button.Position = New Vector2(0, (i + 2 - texts.Count) * 24.0F)
            button.Index = i
            button.IndexY = i
            context.Add(button)
            Select Case i
                Case 0

                    'button.OnFire0 = AddressOf StartButton
                Case 1
                    'button.OnFire0 = AddressOf ExitButton
            End Select
            i += 1
        Next

    End Sub


    Public Overrides Sub LoadContent()
        MyBase.LoadContent()
        naval = content.Load(Of Texture2D)("naval")
        font = content.Load(Of SpriteFont)("fonts/PressStart2P")

        texts = CreateTexts()

        CreateGUIContext(GUIController.CurrentContext)

        updates.Add(Input.Instance)
        updates.Add(guiController)
        Camera.Drawings.Add(guiController)

    End Sub

End Class
