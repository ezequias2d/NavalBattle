Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class MenuScene
    Inherits GameScene

    Private naval As Texture2D

    Private texts As String()
    Private game As Game

    Private contextSizeMap As GUIContext

    Private numberSelectorX As NumberSelector
    Private numberSelectorY As NumberSelector

    Private controlsView As ControlsViewer
    Private labelMove As Label
    Private labelFire0 As Label
    Private labelCancel As Label
    Private labelChangeValue As Label

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
        ControlsViewSizeMap()
    End Sub

    Private Sub OkButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        ScreenManager.Instance.ChangeScene(New NavalBattleScene(numberSelectorX.Value, numberSelectorY.Value))
    End Sub

    Private Sub OnFocusNumberSelector(obj As GUIObject)
        ControlsViewSizeMapSelect()
    End Sub

    Private Sub OnUnfocusNumberSelector(obj As GUIObject)
        ControlsViewSizeMap()
    End Sub

    Private Sub ExitButton(controller As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        game.Exit()
    End Sub

    Private Function CreateContextSizeMap() As GUIContext
        Dim context As GUIContext = New GUIContext(Camera.InternalDimensions)

        numberSelectorX = New NumberSelector(0, 0, 0, Vector2.Zero, 6, 20)
        numberSelectorY = New NumberSelector(1, 0, 1, Vector2.Zero, 6, 20)
        Dim button As Button = New Button(2, 0, 2, Vector2.Zero, "Ok!", Vector2.One)

        Dim gLabelTitle As GUILabel = New GUILabel(3, 0, 3, New Vector2(0, -48.0F), New Label("Map size", Color.White, Label.Font))

        Dim gLabelW As GUILabel = New GUILabel(4, 0, 4, New Vector2(0, -20.0F), New Label("Width", Color.White, Label.Font))
        Dim gLabelH As GUILabel = New GUILabel(5, 0, 5, New Vector2(0, 28.0F), New Label("Height", Color.White, Label.Font))

        gLabelTitle.Scale = Vector2.One * 2.0F
        gLabelW.Scale = Vector2.One * 0.5F
        gLabelH.Scale = Vector2.One * 0.5F

        numberSelectorX.Position = New Vector2(0, 0)
        numberSelectorY.Position = New Vector2(0, 48)

        numberSelectorX.Value = 10
        numberSelectorY.Value = 10
        button.Position = New Vector2(0, numberSelectorY.Position.Y + numberSelectorY.GetSize().Y * 3.0F / 2.0F)

        button.OnFire0 = AddressOf OkButton

        numberSelectorX.OnFocus = AddressOf OnFocusNumberSelector
        numberSelectorY.OnFocus = AddressOf OnFocusNumberSelector

        numberSelectorX.OnUnfocus = AddressOf OnUnfocusNumberSelector
        numberSelectorY.OnUnfocus = AddressOf OnUnfocusNumberSelector

        context.Add(numberSelectorX)
        context.Add(numberSelectorY)
        context.Add(button)
        context.Add(gLabelH)
        context.Add(gLabelW)
        context.Add(gLabelTitle)

        Return context
    End Function

    Private Sub CreateMenu()
        Dim sizePainel As Vector2 = New Vector2(224, 62)

        Dim color As Color = Color.RoyalBlue
        Dim n As Integer = GUIController.MainContext.NextNegative()
        Dim colorPainel As ColorPainel = New ColorPainel(n, 0, n, New Vector2(-sizePainel.X / 2, 16.0F), sizePainel, color)
        GUIController.MainContext.Add(colorPainel)

        n = GUIController.MainContext.NextNegative()
        Dim labelTitle As GUILabel = New GUILabel(n, 0, n, Vector2.Zero, New Label("NavalBattle", Color.Purple, Label.Font))
        labelTitle.Scale = Vector2.One * 1.5F
        GUIController.MainContext.Add(labelTitle)

        Dim i As Integer = 0
        For Each text As String In texts
            Dim button As Button = New Button(i, 0, i, New Vector2(0, (i + 3 - texts.Count) * 32.0F), text, Vector2.One)
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

    Private Sub ControlsViewMainMenuMode()
        GUIController.CurrentContext.Add(controlsView)
        labelMove.DrawEnable = True

        labelFire0.DrawEnable = True
        labelFire0.Text = "Execute"

        labelCancel.DrawEnable = False
        labelChangeValue.DrawEnable = False
    End Sub

    Private Sub ControlsViewSizeMap()
        GUIController.CurrentContext.Add(controlsView)
        labelMove.DrawEnable = True

        labelFire0.DrawEnable = True
        labelFire0.Text = "Select/Execute"

        labelCancel.DrawEnable = True
        labelCancel.Text = "Back"

        labelChangeValue.DrawEnable = False
    End Sub

    Private Sub ControlsViewSizeMapSelect()
        GUIController.CurrentContext.Add(controlsView)
        labelMove.DrawEnable = False
        labelFire0.DrawEnable = False

        labelCancel.DrawEnable = True
        labelCancel.Text = "Deselect"

        labelChangeValue.DrawEnable = True
    End Sub

    Private Sub CreateControlsViewer()
        Dim n As Integer = GUIController.MainContext.NextNegative()
        controlsView = New ControlsViewer(n, 0, n, New Vector2(Camera.InternalDimensions.X / 3, Camera.InternalDimensions.Y * 0.4))
        controlsView.Scale = Vector2.One * 0.5F

        ''Move
        labelMove = New Label("Move", Color.White, Label.Font)

        Dim analogicMove As Sprite = GUIController.CreateAnalogicSprite(Color.BlanchedAlmond)
        Dim up As Sprite = GUIController.CreateUpSprite(Color.BlanchedAlmond)
        Dim down As Sprite = GUIController.CreateDownSprite(Color.BlanchedAlmond)

        Dim moveList As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        moveList.Add((analogicMove, Nothing))
        moveList.Add((down, Nothing))
        moveList.Add((up, Nothing))

        controlsView.Add(labelMove, moveList)

        ''Fire0
        labelFire0 = New Label("Execute", Color.White, Label.Font)

        Dim Fire0 As Sprite = GUIController.CreateFireSprite(Color.GreenYellow)
        Dim orbFire0 As Sprite = GUIController.CreateOrbSprite(Color.GreenYellow)
        Dim orbFire0Label As Label = New Label("B", Color.White, Label.Font)


        Dim fire0List As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        fire0List.Add((Fire0, Nothing))
        fire0List.Add((orbFire0, orbFire0Label))

        controlsView.Add(labelFire0, fire0List)

        ''Cancel
        labelCancel = New Label("Cancel", Color.White, Label.Font)
        labelCancel.DrawEnable = False

        Dim cancel As Sprite = GUIController.CreateFireSprite(Color.Red)
        Dim orbCancel As Sprite = GUIController.CreateOrbSprite(Color.Red)
        Dim orbCancelLabel As Label = New Label("<-", Color.White, Label.Font)


        Dim cancelList As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        cancelList.Add((cancel, Nothing))
        cancelList.Add((orbCancel, orbCancelLabel))

        controlsView.Add(labelCancel, cancelList)

        ''ChangeValue
        labelChangeValue = New Label("Change", Color.White, Label.Font)
        labelChangeValue.DrawEnable = False

        Dim left As Sprite = GUIController.CreateLeftSprite(Color.DarkBlue)
        Dim right As Sprite = GUIController.CreateRightSprite(Color.DarkRed)

        Dim changeList As IList(Of (Sprite, Label)) = New List(Of (Sprite, Label))
        changeList.Add((right, Nothing))
        changeList.Add((left, Nothing))

        controlsView.Add(labelChangeValue, changeList)

        GUIController.MainContext.Add(controlsView)
    End Sub


    Public Overrides Sub LoadContent()
        MyBase.LoadContent()
        naval = content.Load(Of Texture2D)("naval")

        texts = CreateTexts()

        CreateMenu()
        CreateControlsViewer()

        contextSizeMap = CreateContextSizeMap()
    End Sub

End Class
